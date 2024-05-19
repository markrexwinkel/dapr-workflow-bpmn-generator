using Microsoft.CodeAnalysis;
using Rex.Bpmn.Abstractions;
using Rex.Bpmn.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Rex.Bpmn.Dapr.Workflow.Generator;

[Generator]
public partial class BpmnWorkflowGenerator : ISourceGenerator
{
    private const string DiagnosticCategory = "Rex.Bpmn.Dapr.Workflow";
    private static readonly DiagnosticDescriptor FailedToLoadBpmn = new("BPMN0001", "Failed to load BPMN file", "Failed to load BPMN file {0}", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor NoStartEventFound = new("BPMN0002", "No start event found", "No start event found", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor MoreThanOneOutgoingWithoutCondition = new("BPMN0003", "More than one outgoing flow without condition", "There are more than one ({0}) outgoing flows without condition from element {1}, with condition: {2}, default: {3}", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor TargetDoesNotExist = new("BPMN0004", "Target not found", "Target {0} found for flow {1} and source {2} not found", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor ElementNotSupported = new("BPMN0005", "Element not supported", "Element {0} is not supported", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor ExpressionTypeNotSupported = new("BPMN0006", "Expression type not supported", "Expression type {0} not supported", DiagnosticCategory, DiagnosticSeverity.Warning, true);
    private static readonly DiagnosticDescriptor NoInputParameterDefined = new("BPMN0007", "No input parameter defined", "No input parameter defined for element {0}", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor NoOutputParameterDefined = new("BPMN0008", "No output parameter defined", "No output parameter defined for element {0}", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor ItemChoiceTypeNotSupported = new("BPMN0009", "TimerEventDefinition expression not supported", "TimerEventDefinition expression not supported for element {0}", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor LinkTargetNotFound = new("BPMN0010", "Link target not found", "Link target {0} for source {1} not found", DiagnosticCategory, DiagnosticSeverity.Error, true);
    
    public void Execute(GeneratorExecutionContext context)
    {
        try
        {
            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.rootnamespace", out var rootNamespace);

            var bpmnFiles = context.AdditionalFiles.Where(x => Path.GetExtension(x.Path).Equals(".bpmn", StringComparison.OrdinalIgnoreCase));

            foreach (var bpmnFile in bpmnFiles)
            {
                var xml = bpmnFile.GetText().ToString();
                var model = BpmnModel.Parse(xml);
                var definitions = model.Definitions;
                if (definitions is null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(FailedToLoadBpmn, null, bpmnFile));
                    continue;
                }
                foreach (var process in definitions.RootElements.OfType<Process>())
                {
                    var ctx = new GenerateWorkflowContext
                    {
                        GeneratorExecutionContext = context,
                        RootNamespace = rootNamespace,
                        Process = process,
                        Definitions = definitions,
                        Xml = xml,
                    };
                    GenerateWorkflow(ctx);
                    GenerateWorkflowStateClass(ctx);
                    GenerateLogActivityClass(ctx);
                    GenerateWorkflowExtensionClass(ctx);
                    GenerateControllerClass(ctx);
                }
            }
        }
        catch(DiagnosticException ex)
        {
            context.ReportDiagnostic(ex.Diagnostic);
        }
    }

    private static readonly List<Type> DoNotGenerateElements = [typeof(BoundaryEvent)];

    private void GenerateWorkflow(GenerateWorkflowContext ctx)
    {
        var inputParameter = ctx.Process.GetDaprInputParameters().FirstOrDefault();
        var outputParameter = ctx.Process.GetDaprOutputParameters().FirstOrDefault();
        // TODO: validate parameters

        ctx.WorkflowInputType = inputParameter?.Type ?? "object";
        ctx.WorkflowOutputType = outputParameter?.Type ?? "object";

        if(inputParameter is not null)
        {
            ctx.WorkflowInputParameter = inputParameter;
            ctx.WorkflowStateProperties.Add(inputParameter);
        }
        if(outputParameter is not null)
        {
            ctx.WorkflowOutputParameter = outputParameter;
            ctx.WorkflowStateProperties.Add(outputParameter);
        }
        
        var startElement = ctx.Process.GetFlowElement<StartEvent>() ?? throw new DiagnosticException(NoStartEventFound, null);
                
        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine($$"""
            using {{ctx.RootNamespace}}.Workflows.Activities;
            using Rex.Bpmn.Dapr.Workflow;
            using Rex.Bpmn.Dapr.Workflow.Activities;
            using Dapr.Workflow;
            using System;
            using System.Collections.Generic;
            using System.Collections.Concurrent;
            using Microsoft.Extensions.Logging;
            using System.Threading;
            using System.Threading.Tasks;

            namespace {{ctx.RootNamespace}}.Workflows
            {
                public class {{ctx.WorkflowClassName}} : BpmnWorkflow<{{ctx.WorkflowInputType}}, {{ctx.WorkflowOutputType}}, {{ctx.WorkflowStateClassName}}, {{ctx.WorkflowClassName}}>, IBpmnXmlProvider
                {
                    private const string _xml = {{ctx.Xml.ToLiteral()}};

                    protected override async Task<{{ctx.WorkflowOutputType}}> RunInternalAsync(WorkflowContext context, {{ctx.WorkflowInputType}} input)
                    {
                        try
                        {
                            var state = new {{ctx.WorkflowStateClassName}}
                            {
                                {{(inputParameter is null ? string.Empty : $"{inputParameter.Name} = input,")}}
                                {{(outputParameter is null ? string.Empty : $"{outputParameter.Name} = new()")}}
                            };

                            var tasks = new List<Task<CallHandlerResult[]>>() { ExecuteActivityAsync(Call{{startElement.Id}}Async, context, state, null, "{{startElement.Id}}") };
                            while(tasks.Count > 0)
                            {
                                Task<CallHandlerResult[]> readyTask = null;
                                try
                                {
                                    readyTask = await Task.WhenAny(tasks);
                                    tasks.AddRange((await readyTask).Select(x => ExecuteActivityAsync(x.Next, context, state, x.FlowId, x.ActivityId)));
                                }
                                finally
                                {
                                    if (readyTask is not null)
                                    {
                                        tasks.Remove(readyTask);
                                        readyTask = null;
                                    }
                                }
                            }
                            return {{(outputParameter is null ? "null" : $"state.{outputParameter.Name}")}};
                        }
                        catch(Exception ex)
                        {
                            await LogAsync(context, $"Exception occured: {ex}");
                            throw;
                        }
                    }

                    private async Task<object> LogAsync(WorkflowContext context, string message)
                    {
                        return await context.CallActivityAsync<object>(nameof({{ctx.WorkflowClassName}}LogActivity), message);
                    }

                    public static string GetXml() => _xml;
            """);

        foreach (var elem in ctx.Process.FlowElements.OfType<FlowNode>().Where(x => !DoNotGenerateElements.Contains(x.GetType())))
        {
            var src = GenerateElement(ctx, elem);
            if(!string.IsNullOrEmpty(src))
            {
                sourceBuilder.AppendLine();
                sourceBuilder.AppendLine(src.Indent(2));
            }
        }

        sourceBuilder.AppendLine("""
                }
            }
            """);
        ctx.WorkflowClasses.Add(ctx.WorkflowClassName);
        ctx.GeneratorExecutionContext.AddSource($"{ctx.WorkflowClassName}.g.cs", sourceBuilder.ToString());
    }

    private class MethodResult(string body, bool isAsync, bool generateOutgoingFlows)
    {
        public string Body { get; } = body;
        public bool IsAsync { get; } = isAsync;
        public bool GenerateOutgoingFlows { get; } = generateOutgoingFlows;
    }

    private string GenerateElement(GenerateWorkflowContext ctx, FlowNode elem)
    {
        var boundaryEvents = GetBoundaryEventDefinitions(ctx.Definitions, ctx.Process, elem, out var timeout);
        var outputParameter = elem.GetDaprOutputParameters().FirstOrDefault();
        if (outputParameter is not null)
        {
            ctx.WorkflowStateProperties.Add(outputParameter);
        }
        var method = elem switch
        {
            ServiceTask or ScriptTask => GenerateActivityTask(ctx, elem, outputParameter, timeout),
            ExclusiveGateway exclusiveGateway => GenerateExclusiveGateway(ctx, exclusiveGateway),
            InclusiveGateway inclusiveGateway => GenerateInclusiveGateway(ctx, inclusiveGateway),
            ParallelGateway parallelGateway => GenerateParallelGateway(parallelGateway),
            EndEvent => GenerateEndEvent(),
            UserTask userTask => GenerateUserTask(userTask, outputParameter, timeout),
            ReceiveTask receiveTask => GenerateReceiveTask(ctx, receiveTask, outputParameter, timeout),
            IntermediateCatchEvent catchEvent => GenerateIntermediateCatchEvent(ctx, catchEvent, outputParameter, timeout),
            IntermediateThrowEvent throwEvent => GenerateIntermediateThrowEvent(ctx, throwEvent),
            StartEvent => GenerateStartEvent(),
            SubProcess subProcess => GenerateSubProcess(ctx, subProcess, timeout),
            CallActivity callActivity => GenerateBpmnCallActivity(callActivity, outputParameter, timeout),
            ManualTask => new(string.Empty, false, true),
            _ => throw new DiagnosticException(ElementNotSupported, null, elem?.GetType().FullName)
        };

        if(method is null)
        {
            return string.Empty;
        }

        var methodSignature = GenerateFlowNodeMethodSignature(ctx, elem, method.IsAsync);
        var outgoingMethods = method.GenerateOutgoingFlows ? GenerateOutgoingFlows(ctx, elem) : string.Empty;
        var returnOutgoingMethods = outgoingMethods.Length > 0 ? GetReturnStatement(outgoingMethods, method.IsAsync) : string.Empty;
        
        if (boundaryEvents.Count > 0)
        {
            var catchBodies = new StringBuilder();
            foreach (var def in boundaryEvents)
            {
                var catchStatement = def.Definition switch
                {
                    TimerEventDefinition => "catch(TaskCanceledException)",
                    _ => "catch"
                };
                catchBodies.AppendLine($$"""
                    {{catchStatement}}
                    {
                        {{GetReturnStatement(GenerateOutgoingFlows(ctx, def.Event), method.IsAsync)}}
                    }
                    """);
            }
            return $$"""
                {{methodSignature}}
                {
                    try
                    {
                {{string.Join("\r\n", method.Body, returnOutgoingMethods).Trim().Indent(2)}}
                    }
                {{catchBodies.ToString().Indent(1)}}
                }
                """;
        }
        else
        {
            return $$"""
                {{methodSignature}}
                {
                {{string.Join("\r\n", method.Body, returnOutgoingMethods).Trim().Indent(1)}}
                }
                """;
        }

        static string GetReturnStatement(string outgoingMethods, bool isAsync)
        {
            return $"return {(!isAsync ? "Task.FromResult<CallHandlerResult[]>(" : string.Empty)}new CallHandlerResult[] {{{outgoingMethods}}}{(!isAsync ? ")" : string.Empty)};";
        }
    }

    private string GenerateFlowNodeMethodSignature(GenerateWorkflowContext ctx, FlowNode flowNode, bool isAsync = true)
    {
        return $"private {(isAsync ? "async " : "")}Task<CallHandlerResult[]> Call{flowNode.Id}Async(WorkflowContext context, {ctx.WorkflowStateClassName} state, CallHandlerContext callContext)";
    }

    private string GenerateWaitForExternalEvent(string messageName, DaprParameter outputParameter, TimeSpan? timeout)
    {
        var outputType = outputParameter is null ? "object" : outputParameter.Type;
        var outputAssignment = outputParameter is not null ? $"state.{outputParameter.Name} = " : string.Empty;

        return $"{outputAssignment}await context.WaitForExternalEventAsync<{outputType}>(\"{messageName}\"{(timeout.HasValue ? $", TimeSpan.FromTicks({timeout.Value.Ticks})" : "")});";
    }

    private string GenerateCallActivity(string className, DaprParameter outputParameter, TimeSpan? timeout)
    {
        var outputType = outputParameter is null ? "object" : outputParameter.Type;
        var outputAssignment = outputParameter is not null ? $"state.{outputParameter.Name} = " : string.Empty;

        if (timeout.HasValue)
        {
            return $$"""
                var timerTask = context.CreateTimer(TimeSpan.FromTicks({{timeout.Value.Ticks}}));
                var activityTask = context.CallActivityAsync<{{outputType}}>(nameof({{className}}), state);
                var task = await Task.WaitAny(timerTask, activityTask);
                if (task == timerTask)
                {
                    throw new TaskCanceledException();
                }
                else
                {
                    {{outputAssignment}}await activityTask;
                }
                """;
        }
        else
        {
            return $"{outputAssignment}await context.CallActivityAsync<{outputType}>(nameof({className}), state);";
        }
    }

    private MethodResult GenerateSubProcess(GenerateWorkflowContext ctx, SubProcess subProcess, TimeSpan? timeout)
    {
        var subProcessContext = ctx.ForSubProcess(subProcess);
        GenerateWorkflow(subProcessContext);
        GenerateWorkflowStateClass(subProcessContext);
        GenerateLogActivityClass(subProcessContext);

        var assignment = subProcessContext.WorkflowOutputParameter is null ? string.Empty : $"state.{subProcessContext.WorkflowOutputParameter.Name} = ";
        var input = subProcessContext.WorkflowInputParameter is null ? "null" : $"state.{subProcessContext.WorkflowInputParameter.Name}";
        string src;

        if (timeout.HasValue)
        {
            src = $$"""
                var timerTask = context.CreateTimer(TimeSpan.FromTicks({{timeout.Value.Ticks}}));
                var activityYask = context.CallChildWorkflowAsync<{{subProcessContext.WorkflowOutputType}}>(nameof({{subProcessContext.WorkflowClassName}}), {{input}});
                var task = await Task.WaitAny(timerTask, activityTask);
                if (task == timerTask)
                {
                    throw new TaskCanceledException();
                }
                else
                {
                    {{assignment}}await activityTask;
                }
                """;
        }
        else
        {
            src = $"{assignment}await context.CallChildWorkflowAsync<{subProcessContext.WorkflowOutputType}>(nameof({subProcessContext.WorkflowClassName}), {input});";
        }
        return new(src, true, true);
    }

    private MethodResult GenerateBpmnCallActivity(CallActivity callActivity, DaprParameter outputParameter, TimeSpan? timeout)
    {
        var outputType = outputParameter is null ? "object" : outputParameter.Type;
        var outputAssignment = outputParameter is not null ? $"state.{outputParameter.Name} = " : string.Empty;
        string src;

        if (timeout.HasValue)
        {
            src = $$"""
                var timerTask = context.CreateTimer(TimeSpan.FromTicks({{timeout.Value.Ticks}}));
                var activityTask = context.CallChildWorkflowAsync<{{outputType}}>(nameof({{callActivity.CalledElement}}), state);
                var task = await Task.WaitAny(timerTask, activityTask);
                if (task == timerTask)
                {
                    throw new TaskCanceledException();
                }
                else
                {
                    {{outputAssignment}}await activityTask;
                }
                """;
        }
        else
        {
            src = $"{outputAssignment}await context.CallActivityAsync<{outputType}>(nameof({callActivity.CalledElement}), state);";
        }
        return new(src, true, true);
    }

    private MethodResult GenerateUserTask(UserTask userTask, DaprParameter outputParameter, TimeSpan? timeout)
    {
        return new(GenerateWaitForExternalEvent($"{userTask.Id}Completed", outputParameter, timeout), true, true);
    }

    private MethodResult GenerateReceiveTask(GenerateWorkflowContext ctx, ReceiveTask receiveTask, DaprParameter outputParameter, TimeSpan? timeout)
    {
        var messageName = ctx.Definitions.RootElements.OfType<Message>().FirstOrDefault(x => x.Id == $"{receiveTask.MessageRef}")?.Name ?? $"{receiveTask.Id}Completed";
        return new(GenerateWaitForExternalEvent(messageName, outputParameter, timeout), true, true);
    }

    

    private MethodResult GenerateActivityTask(GenerateWorkflowContext ctx, FlowElement task, DaprParameter outputParameter, TimeSpan? timeout)
    {
        var className = GenerateActivityClass(ctx, task, outputParameter);
        return new(GenerateCallActivity(className, outputParameter, timeout), true, true);
    }

    private MethodResult GenerateEndEvent()
    {
        return new("return Task.FromResult(Array.Empty<CallHandlerResult>());", false, false);
    }

    private MethodResult GenerateStartEvent() => new("", false, true);
    
    private MethodResult GenerateParallelGateway(ParallelGateway parallelGateway)
    {
        var incomingFlowIds = parallelGateway.Incoming.Select(x => x.ToString()).ToList();
        var src = $$"""
            var incomingFlowIds = new List<string>() { {{ string.Join(", ", incomingFlowIds.Select(x => $"\"{x}\"")) }} };
            
            if(callContext.EntryId > 0)
            {
                await LogAsync(context, $"[Parallel Gateway {{parallelGateway.Id}}][{callContext.EntryId}] Already waiting send event for flow {callContext.FlowId}");
                await context.CallActivityAsync<object>(nameof(SendLocalEventActivity), new SendLocalEvent { Message = callContext.FlowId });
                return Array.Empty<CallHandlerResult>();
            }
            
            var waitForFlowIds = incomingFlowIds.Where(x => x != callContext.FlowId).ToList();
            if(waitForFlowIds.Count > 0)
            {
                await LogAsync(context, $"[Parallel Gateway {{parallelGateway.Id}}][{callContext.EntryId}] Waiting for flows {string.Join(", ", waitForFlowIds)}");
                await Task.WhenAll(waitForFlowIds.Select(x => context.WaitForExternalEventAsync<object>(x)));
            }
            else
            {
                await LogAsync(context, "[Parallel Gateway {{parallelGateway.Id}}][{callContext.EntryId}] no flows to wait for, returning outgoing flows");
            }
            """;
        return new(src, true, true);
    }

    private MethodResult GenerateInclusiveGateway(GenerateWorkflowContext ctx, InclusiveGateway inclusiveGateway)
    {
        var incomingFlowIds = inclusiveGateway.Incoming.Select(x => x.ToString()).ToList();
        var outgoingFlows = ctx.Process.FlowElements.OfType<SequenceFlow>().Where(x => inclusiveGateway.Outgoing.Any(y => y.ToString() == x.Id)).ToList();
        
        var outgoingFlowsBuilder = new StringBuilder();

        foreach(var outgoingFlow in outgoingFlows)
        {
            var target = ctx.Process.FlowElements.FirstOrDefault(x => x.Id == outgoingFlow.TargetRef) ?? throw new DiagnosticException(TargetDoesNotExist, null, outgoingFlow.TargetRef, outgoingFlow.Id, outgoingFlow.SourceRef);
            var condition = outgoingFlow.ConditionExpression.Text.FirstOrDefault();
            if (string.IsNullOrEmpty(condition))
            {
                outgoingFlowsBuilder.AppendLine($"outgoingCalls.Add(new CallHandlerResult(Call{target.Id}Async, \"{outgoingFlow.Id}\", \"{target.Id}\"));");
            }
            else
            {
                outgoingFlowsBuilder.AppendLine($$"""
                    if (state.{{condition}})
                    {
                        outgoingCalls.Add(new CallHandlerResult(Call{{target.Id}}Async, "{{outgoingFlow.Id}}", "{{target.Id}}"));
                    }
                    """);
            }
        }

        var src = $$"""
            var incomingFlowIds = new List<string>() { {{string.Join(", ", incomingFlowIds.Select(x => $"\"{x}\""))}} };
            
            if(callContext.EntryId > 0)
            {
                await LogAsync(context, $"[Parallel Gateway {{inclusiveGateway.Id}}][{callContext.EntryId}] Already waiting send event for flow {callContext.FlowId}");
                await context.CallActivityAsync<object>(nameof(SendLocalEventActivity), new SendLocalEvent { Message = callContext.FlowId});
                return Array.Empty<CallHandlerResult>();
            }

            
            var waitForFlowIds = incomingFlowIds.Where(x => x != callContext.FlowId).ToList();
            if(waitForFlowIds.Count > 0)
            {
                await LogAsync(context, $"[Parallel Gateway {{inclusiveGateway.Id}}][{callContext.EntryId}] Waiting for flows {string.Join(", ", waitForFlowIds)}");
                await Task.WhenAll(waitForFlowIds.Select(x => context.WaitForExternalEventAsync<object>(x)));
            }
            else
            {
                await LogAsync(context, "[Parallel Gateway {{inclusiveGateway.Id}}][{callContext.EntryId}] no flows to wait for, returning outgoing flows");
            }
            var outgoingCalls = new List<CallHandlerResult[]>();

            {{outgoingFlowsBuilder.ToString().Indent(1)}}

            return outgoingCalls.ToArray();
            """;
        return new(src, true, false);
    }

    private MethodResult GenerateExclusiveGateway(GenerateWorkflowContext ctx, ExclusiveGateway exclusiveGateway)
    {
        var outgoingFlows = ctx.Process.FlowElements.OfType<SequenceFlow>().Where(x => exclusiveGateway.Outgoing.Any(y => y.ToString() == x.Id)).ToList();
        var defaultFlow = outgoingFlows.FirstOrDefault(x => x.Id == exclusiveGateway.Default);
        var conditionFlows = outgoingFlows.Where(x => x.ConditionExpression != null).ToList();
        var nonConditionFlows = outgoingFlows.Except(conditionFlows).ToList();

        if ((outgoingFlows.Count > 1 && conditionFlows.Count == 0) || (defaultFlow is not null && nonConditionFlows.Count > 1))
        {
            throw new DiagnosticException(MoreThanOneOutgoingWithoutCondition, null, outgoingFlows.Count, exclusiveGateway.Id, conditionFlows.Count, defaultFlow?.Id ?? "<none>");
        }
        var sourceBuilder = new StringBuilder();
                
        for (var i = 0; i < conditionFlows.Count; i++)
        {
            var target = ctx.Process.FlowElements.FirstOrDefault(x => x.Id == conditionFlows[i].TargetRef) ?? throw new DiagnosticException(TargetDoesNotExist, null, conditionFlows[i].TargetRef, conditionFlows[i].Id, conditionFlows[i].SourceRef);
            sourceBuilder.AppendLine($$"""
                {{(i > 0 ? "else " : "")}}if (state.{{conditionFlows[i].ConditionExpression.Text.FirstOrDefault()}})
                {
                    return Task.FromResult<CallHandlerResult[]>(new CallHandlerResult[] { new CallHandlerResult(Call{{target.Id}}Async, "{{outgoingFlows[i].Id}}", "{{target.Id}}") });
                }
                """);
        }

        var lastFlow = defaultFlow ?? nonConditionFlows.FirstOrDefault();
        if (lastFlow is not null)
        {
            var target = ctx.Process.FlowElements.FirstOrDefault(x => x.Id == lastFlow.TargetRef) ?? throw new DiagnosticException(TargetDoesNotExist, null, lastFlow.TargetRef, lastFlow.Id, lastFlow.SourceRef);
            if (conditionFlows.Count > 0)
            {
                sourceBuilder.AppendLine($$"""
                    else
                    {
                        return Task.FromResult<CallHandlerResult[]>(new CallHandlerResult[] { new CallHandlerResult(Call{{target.Id}}Async, "{{lastFlow.Id}}", "{{target.Id}}") });
                    }
                    """);
            }
            else
            {
                sourceBuilder.AppendLine($$"""
                    return Task.FromResult<CallHandlerResult[]>(new CallHandlerResult[] { new CallHandlerResult(Call{{target.Id}}Async, "{{lastFlow.Id}}", "{{target.Id}}") });
                    """);
            }
        }

        return new(sourceBuilder.ToString(), false, false);
    }

    private MethodResult GenerateIntermediateCatchEvent(GenerateWorkflowContext ctx, IntermediateCatchEvent catchEvent, DaprParameter outputParameter, TimeSpan? timeout)
    {
        var eventDefinition = catchEvent.GetEventDefinition(ctx.Definitions);
        return eventDefinition switch
        {
            TimerEventDefinition ted => GenerateIntermediateTimerCatchEvent(catchEvent, ted),
            MessageEventDefinition med => GenerateIntermediateMessageCatchEvent(ctx, catchEvent, med, outputParameter, timeout),
            ConditionalEventDefinition ced => GenerateIntermediateConditionalCatchEvent(ced),
            LinkEventDefinition => new MethodResult("", false, true),
            //SignalEventDefinition sed => GenerateIntermediateSignalCatchEvent(ctx, catchEvent, sed),
            _ => new("", false, true)
        };
    }

    private MethodResult GenerateIntermediateThrowEvent(GenerateWorkflowContext ctx, IntermediateThrowEvent throwEvent)
    {
        var eventDefinition = throwEvent.GetEventDefinition(ctx.Definitions);
        return eventDefinition switch
        {
            MessageEventDefinition med => GenerateIntermediateMessageThrowEvent(ctx, throwEvent, med),
            LinkEventDefinition led => GenerateIntermediateLinkThrowEvent(ctx, throwEvent, led),
            //SignalEventDefinition sed => GenerateIntermediateSignalThrowEvent(ctx, throwEvent, sed),
            //EscalationEventDefinition eed => GenerateIntermediateEscalationThrowEvent(ctx, throwEvent, eed),
            //CompensateEventDefinition ced => GenerateIntermediateCompensateThrowEvent(ctx, throwEvent, ced),
            _ => new("", false, true)
        };
    }
        
    //private MethodResult GenerateIntermediateSignalCatchEvent(GenerateWorkflowContext ctx, IntermediateCatchEvent catchEvent, SignalEventDefinition sed)
    //{
    //    throw new NotImplementedException();
    //}

    private MethodResult GenerateIntermediateLinkThrowEvent(GenerateWorkflowContext ctx, IntermediateThrowEvent throwEvent, LinkEventDefinition led)
    {

        var target = ctx.Process.FlowElements.OfType<IntermediateCatchEvent>().FirstOrDefault(x => x.Id == led.Target.ToString() && x.GetEventDefinition() is LinkEventDefinition) 
                ?? throw new DiagnosticException(LinkTargetNotFound, null, led.Target.ToString(), throwEvent.Id);
        var src = $"return Task.FromResult<CallHandlerResult[]>(new CallHandlerResult[] {{ new CallHandlerResult(Call{target.Id}Async, null, \"{target.Id}\") }});";
        return new(src, false, false);
    }

    private MethodResult GenerateIntermediateConditionalCatchEvent(ConditionalEventDefinition ced)
    {
        var condition = ced.Condition.Text[0];
        var src = $$"""
            if(!({{condition}}))
            {
                return Task.FromResult<CallHandlerResult[]>(Array.Empty<CallHandlerResult>());
            }
            """;
        return new(src, false, true);
    }

    private string GenerateSendLocalEvent(string messageName, string input = "state")
    {
        return $"await context.CallActivityAsync<object>(nameof(SendLocalEventActivity), new SendLocalEvent {{ Message = \"{messageName}\", Input = {input} }});";
    }

    private MethodResult GenerateIntermediateMessageThrowEvent(GenerateWorkflowContext ctx, ThrowEvent throwEvent, MessageEventDefinition med)
    {
        var messageName = ctx.Definitions.RootElements.OfType<Message>().FirstOrDefault(x => x.Id == $"{med.MessageRef}")?.Name ?? $"{throwEvent.Id}";
        return new(GenerateSendLocalEvent(messageName), false, true);
    }

    private MethodResult GenerateIntermediateMessageCatchEvent(GenerateWorkflowContext ctx, IntermediateCatchEvent catchEvent, MessageEventDefinition med, DaprParameter outputParameter, TimeSpan? timeout)
    {
        var messageName = ctx.Definitions.RootElements.OfType<Message>().FirstOrDefault(x => x.Id == $"{med.MessageRef}")?.Name ?? $"{catchEvent.Id}";
        return new(GenerateWaitForExternalEvent(messageName, outputParameter, timeout), true, true);
    }

    private MethodResult GenerateIntermediateTimerCatchEvent(IntermediateCatchEvent catchEvent, TimerEventDefinition ted)
    {
        var itemText = ted.Item.Text[0];
        
        var arg = ted.ItemElementName switch
        {
            ItemChoiceType.TimeDate => $"DateTime.ParseExact(\"{itemText}\", \"O\", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)",
            ItemChoiceType.TimeDuration => $"XmlConvert.ToTimeSpan(\"{itemText}\")",
            _ => throw new DiagnosticException(ItemChoiceTypeNotSupported, null, catchEvent.Id)
        };
        var src = $$"""
            await context.CreateTimer({{arg}});
            """;
        return new(src, true, true);
    }

    private List<(BoundaryEvent Event, EventDefinition Definition)> GetBoundaryEventDefinitions(Definitions definitions, IFlowElements process, FlowNode flowNode, out TimeSpan? timeout)
    {
        var events = process.FlowElements.OfType<BoundaryEvent>().Where(x => x.AttachedToRef.ToString() == flowNode.Id);
        var supportedTypes = new List<Type> { typeof(TimerEventDefinition), typeof(ErrorEventDefinition) };

        var defs = from e in events
                   let d = e.GetEventDefinition(definitions)
                   where supportedTypes.Contains(d.GetType())
                   orderby supportedTypes.IndexOf(d.GetType())
                   select (e, d);
                
        timeout = GetTimeSpanFromDefinition(defs.Select(x => x.d).OfType<TimerEventDefinition>().FirstOrDefault());

        return defs.ToList();
    }

    private TimeSpan? GetTimeSpanFromDefinition(TimerEventDefinition timerDefinition)
    {
        return timerDefinition?.ItemElementName switch
        {
            ItemChoiceType.TimeDuration => (TimeSpan?)XmlConvert.ToTimeSpan(timerDefinition.Item.Text[0]),
            null => null,
            _ => throw new DiagnosticException(ExpressionTypeNotSupported, null, timerDefinition.ItemElementName),
        };
    }

    private string GenerateOutgoingFlows(GenerateWorkflowContext ctx, BaseElement elem)
    {
        var outgoingFlows = ctx.Process.FlowElements.OfType<SequenceFlow>().Where(x => x.SourceRef == elem.Id);
        var targets = from o in outgoingFlows
                      from f in ctx.Process.FlowElements
                      where f.Id == o.TargetRef
                      select $"new CallHandlerResult(Call{f.Id}Async, \"{o.Id}\", \"{f.Id}\")";
        //var targets = ctx.Process.FlowElements.Where(x => outgoingFlows.Any(y => x.Id == y.TargetRef)).Select(x => $"Call{x.Id}Async");
        return string.Join(", ", targets);
    }

    private string GenerateActivityClass(GenerateWorkflowContext ctx, FlowElement activityClass, DaprParameter outputParameter) 
    {
        var id = activityClass.Id.Capatilize();
        var className = $"{id}Activity";

        if (!ctx.ActivityClasses.Contains(className))
        {
            var src = $$"""
            using Dapr.Workflow;
            
            namespace {{ctx.RootNamespace}}.Workflows.Activities
            {
                public partial class {{className}} : WorkflowActivity<{{ctx.WorkflowStateClassName}}, {{outputParameter?.Type ?? "object"}}>
                {
                }
            }
            """;
            ctx.GeneratorExecutionContext.AddSource($"{className}.g.cs", src);
            ctx.ActivityClasses.Add(className);
        }
        return className;
    }

    private void GenerateWorkflowExtensionClass(GenerateWorkflowContext ctx)
    {
        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine($$"""
            using {{ctx.RootNamespace}}.Workflows;
            using {{ctx.RootNamespace}}.Workflows.Activities;
            using Rex.Bpmn.Dapr.Workflow.Activities;
            using Dapr.Workflow;

            namespace Microsoft.Extensions.DependencyInjection
            {
                public static class {{ctx.WorkflowClassName}}ServiceCollectionExtensions
                {
                    public static IServiceCollection Add{{ctx.WorkflowClassName}}(this IServiceCollection services)
                    {
                        services.AddBpmnWorkflow()
            """);
        foreach (var className in ctx.WorkflowClasses)
        {
            sourceBuilder.AppendLine($$"""
                                .TryRegisterWorkflow<{{className}}>()
                """);
        }     
        foreach (var className in ctx.ActivityClasses)
        {
            sourceBuilder.AppendLine($$"""
                                .TryRegisterActivity<{{className}}>()
                """);
        }
        sourceBuilder.AppendLine($$"""
                            .TryRegisterActivity<SendLocalEventActivity>();
            """);
        sourceBuilder.AppendLine($$"""
                        return services;
                    }
                }
            }
            """
        );
        ctx.GeneratorExecutionContext.AddSource($"{ctx.WorkflowClassName}ServiceCollectionExtensions.g.cs", sourceBuilder.ToString());
    }

    private void GenerateControllerClass(GenerateWorkflowContext ctx)
    {
        var controllerClassName = $"{ctx.Process.Id}Controller";
        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine($$"""
            using {{ctx.RootNamespace}}.Workflows;
            using Dapr.Workflow;
            using Microsoft.AspNetCore.Mvc;
            using System;
            using System.Linq;
            using System.Threading.Tasks;
            using Rex.Bpmn.Abstractions;
            using Rex.Bpmn.Drawing;
            using Rex.Bpmn.Dapr.Workflow.Services;

            namespace {{ctx.RootNamespace}}.Controllers
            {
                [ApiController]
                [Route("[controller]")]
                public partial class {{controllerClassName}} : ControllerBase
                {
                    private readonly ILogger<{{controllerClassName}}> _logger;
                    private readonly DaprWorkflowClient _workflowClient;
                    private readonly IWorkflowService _workflowService;

                    public {{controllerClassName}}(ILogger<{{controllerClassName}}> logger, DaprWorkflowClient workflowClient, IWorkflowService workflowService)
                    {
                        _logger = logger;
                        _workflowClient = workflowClient;
                        _workflowService = workflowService;
                    }

                    [HttpPost("")]
                    public async Task<string> NewWorkflowAsync({{ctx.WorkflowInputType}} state)
                    {
                        var instanceId = Guid.NewGuid().ToString("N");
                        await _workflowClient.ScheduleNewWorkflowAsync(
                            name: nameof({{ctx.WorkflowClassName}}),
                            instanceId: instanceId,
                            input: state);
                        return instanceId;
                    }

                    [HttpGet("xml")]
                    public IActionResult GetWorkflowXml() => Content({{ctx.WorkflowClassName}}.GetXml(), "text/xml");

                    [HttpGet("diagram")]
                    public IActionResult GetDiagram()
                    {
                        var xml = {{ctx.WorkflowClassName}}.GetXml();
                        var model = BpmnModel.Parse(xml);
                        var diagram = model.Definitions.BpmnDiagrams.FirstOrDefault();
                        if(diagram is null)
                        {
                            return NotFound();
                        }
                        var stream = new MemoryStream();
                        diagram.WriteSvg(model.Definitions, stream);
                        stream.Position = 0;
                        return File(stream, "image/svg+xml", "{{ctx.Process.Id}}.svg");
                    }

                    [HttpGet("{instanceId}/diagram")]
                    public async Task<IActionResult> GetDiagramAsync(string instanceId)
                    {
                        var xml = {{ctx.WorkflowClassName}}.GetXml();
                        var model = BpmnModel.Parse(xml);
                        var diagram = model.Definitions.BpmnDiagrams.FirstOrDefault();
                        if(diagram is null)
                        {
                            return NotFound();
                        }
                        var activityState = await _workflowService.GetActivityStateAsync(instanceId);
                        var stream = new MemoryStream();
                        diagram.WriteSvg(model.Definitions, stream, activityState.Tokens);
                        stream.Position = 0;
                        return File(stream, "image/svg+xml", "{{ctx.Process.Id}}.svg");
                    }
                    
            """);

        foreach (var elem in ctx.Process.FlowElements.Where(x => x is ReceiveTask || x is UserTask))
        {
            var messageName = elem switch
            {
                ReceiveTask rt => ctx.Definitions.RootElements.OfType<Message>().FirstOrDefault(x => x.Id == $"{rt.MessageRef}")?.Name ?? $"{rt.Id}Completed",
                UserTask ut => $"{ut.Id}Completed",
                _ => null
            };

            if (messageName is not null)
            {
                var type = elem.GetDaprOutputParameters().FirstOrDefault()?.Type ?? "object";
                sourceBuilder.AppendLine($$"""
                            [HttpPost("{instanceId}/Raise{{messageName}}")]
                            public async Task<IActionResult> Raise{{messageName}}Async(string instanceId, [FromBody] {{type}} input)
                            {
                                await _workflowClient.RaiseEventAsync(
                                    instanceId: instanceId,
                                    eventName: "{{messageName}}",
                                    eventPayload: input);
                                return Ok();
                            }
                    """);
            }
        }

        sourceBuilder.AppendLine("""
                }
            }
            """);
        ctx.GeneratorExecutionContext.AddSource($"{controllerClassName}.g.cs", sourceBuilder.ToString());
    }

    private void GenerateLogActivityClass(GenerateWorkflowContext ctx)
    {
        var src = $$"""
            using Dapr.Workflow;
            using Microsoft.Extensions.Logging;

            namespace {{ctx.RootNamespace}}.Workflows.Activities
            {
                public partial class {{ctx.WorkflowClassName}}LogActivity : WorkflowActivity<string, object>
                {
                    private readonly ILogger<{{ctx.WorkflowClassName}}> _logger;

                    public {{ctx.WorkflowClassName}}LogActivity(ILogger<{{ctx.WorkflowClassName}}> logger)
                    {
                        _logger = logger;
                    }

                    public override Task<object> RunAsync(WorkflowActivityContext context, string message)
                    {
                        _logger.LogInformation($"[Workflow {context.InstanceId}] {message}");
                        return Task.FromResult<object>(null);
                    }
                }
            }
            """;
        ctx.GeneratorExecutionContext.AddSource($"{ctx.WorkflowClassName}LogActivity.g.cs", src);
        ctx.ActivityClasses.Add($"{ctx.WorkflowClassName}LogActivity");
    }

        
    private void GenerateWorkflowStateClass(GenerateWorkflowContext ctx)
    {
        var propDefs = string.Join("\r\n", ctx.WorkflowStateProperties
            .Where(x => !x.Name.Contains("."))
            .Select(x => $"public {x.Type} {x.Name} {{ get; set; }}")
            .Distinct());

        var src = $$"""
            namespace {{ctx.RootNamespace}}.Workflows
            {
                public partial class {{ctx.WorkflowStateClassName}}
                {
            {{propDefs.Indent(2)}}
                }
            }
            """;

        ctx.GeneratorExecutionContext.AddSource($"{ctx.WorkflowStateClassName}.g.cs", src);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Nothing to initialize
    }
}
