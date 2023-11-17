using Microsoft.CodeAnalysis;
using Rex.Bpmn.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Dapr.Workflow.Bpmn;

[Generator]
public partial class BpmnWorkflowGenerator : ISourceGenerator
{
    private readonly XmlSerializer _bpmnSerializer = new(typeof(Definitions));
    private const string DiagnosticCategory = "Rex.Dapr.Workflow.Bpmn";
    private static readonly DiagnosticDescriptor FailedToLoadBpmn = new("BPMN0001", "Failed to load BPMN file", "Failed to load BPMN file {0}", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor NoStartEventFound = new("BPMN0002", "No start event found", "No start event found", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor MoreThanOneOutgoingWithoutCondition = new("BPMN0003", "More than one outgoing flow without condition", "There are more than one ({0}) outgoing flows without condition from element {1}, with condition: {2}, default: {3}", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor TargetDoesNotExist = new("BPMN0004", "Target not found", "Target {0} found for flow {1} and source {2} not found", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor ElementNotSupported = new("BPMN0005", "Element not supported", "Element {0} is not supported", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor ExpressionTypeNotSupported = new("BPMN0006", "Expression type not supported", "Expression type {0} not supported", DiagnosticCategory, DiagnosticSeverity.Warning, true);

    public void Execute(GeneratorExecutionContext context)
    {
        try
        {
            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.rootnamespace", out var rootNamespace);

            var bpmnFiles = context.AdditionalFiles.Where(x => Path.GetExtension(x.Path).Equals(".bpmn", StringComparison.OrdinalIgnoreCase));

            foreach (var bpmnFile in bpmnFiles)
            {
                var xml = bpmnFile.GetText().ToString();
                var definitions = (Definitions)_bpmnSerializer.Deserialize(new StringReader(xml));
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
                        WorkflowClassName = $"{process.Id}Workflow",
                        Process = process,
                        Definitions = definitions,
                    };
                    GenerateWorkflow(ctx);
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
        var startElement = ctx.Process.GetFlowElement<StartEvent>() ?? throw new DiagnosticException(NoStartEventFound, null);
                
        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine($$"""
            using {{ctx.RootNamespace}}.Workflows.Activities;
            using Dapr.Workflow;
            using Rex.Dapr.Workflow.Bpmn;
            using System;
            using System.Collections.Generic;
            using Microsoft.Extensions.Logging;
           
            namespace {{ctx.RootNamespace}}.Workflows
            {
                public class {{ctx.WorkflowClassName}} : Workflow<BpmnWorkflowState, BpmnWorkflowState>
                {
                    private delegate Task<CallHandlerAsync[]> CallHandlerAsync(WorkflowContext context, BpmnWorkflowState state);

                    public override async Task<BpmnWorkflowState> RunAsync(WorkflowContext context, BpmnWorkflowState state)
                    {
                        var tasks = new List<Task<CallHandlerAsync[]>>() { Call{{startElement.Id}}Async(context, state) };
                        while(tasks.Count > 0)
                        {
                            var readyTask = await Task.WhenAny(tasks);
                            tasks.AddRange((await readyTask).Select(x => x(context, state)));
                        }
                        return state;
                    }
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
        ctx.GeneratedClasses.Add(ctx.WorkflowClassName);
        ctx.GeneratorExecutionContext.AddSource($"{ctx.WorkflowClassName}.g.cs", sourceBuilder.ToString());
    }

    private class MethodResult
    {
        public MethodResult(string body, bool isAsync, bool generateOutgoingFlows)
        {
            Body = body;
            IsAsync = isAsync;
            GenerateOutgoingFlows = generateOutgoingFlows;
        }

        public string Body { get; }
        public bool IsAsync { get; }
        public bool GenerateOutgoingFlows { get; }
    }

    private string GenerateElement(GenerateWorkflowContext ctx, FlowNode elem)
    {
        var boundaryEvents = GetBoundaryEventDefinitions(ctx.Process, elem, out var timeout);
        var method = elem switch
        {
            ServiceTask serviceTask => GenerateServiceTask(ctx, serviceTask, timeout),
            ExclusiveGateway exclusiveGateway => GenerateExclusiveGateway(ctx, exclusiveGateway),
            EndEvent => GenerateEndEvent(),
            UserTask userTask => GenerateUserTask(userTask, timeout),
            ReceiveTask receiveTask => GenerateReceiveTask(ctx, receiveTask, timeout),
            StartEvent => GenerateStartEvent(),
            _ => throw new DiagnosticException(ElementNotSupported, null, elem?.GetType().FullName)
        };

        if(method is null)
        {
            return string.Empty;
        }

        var methodSignature = GenerateFlowNodeMethodSignature(elem, method.IsAsync);
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
            return $"return {(!isAsync ? "Task.FromResult<CallHandlerAsync[]>(" : string.Empty)}new CallHandlerAsync[] {{{outgoingMethods}}}{(!isAsync ? ")" : string.Empty)};";
        }
    }

    private string GenerateFlowNodeMethodSignature(FlowNode flowNode, bool isAsync = true)
    {
        return $"private {(isAsync ? "async " : "")}Task<CallHandlerAsync[]> Call{flowNode.Id}Async(WorkflowContext context, BpmnWorkflowState state)";
    }

    private string GenerateWaitForExternalEvent(string messageName, TimeSpan? timeout)
    {
        return $"state.Merge(await context.WaitForExternalEventAsync<BpmnWorkflowState>(\"{messageName}\"{(timeout.HasValue ? $", TimeSpan.FromTicks({timeout.Value.Ticks})" : "")}));";
    }

    private string GenerateCallActivity(string className, TimeSpan? timeout)
    {
        if(timeout.HasValue)
        {
            return $$"""
                var timerTask = context.CreateTimer(TimeSpan.FromTicks({{timeout.Value.Ticks}}));
                var activityTask = context.CallActivityAsync<BpmnWorkflowState>(nameof({{className}}), state);
                var task = await Task.WaitAny(timerTask, activityTask);
                if (task == timerTask)
                {
                    throw new TaskCanceledException();
                }
                state.Merge(await activityTask);
                """;
        }
        else
        {
            return $"state.Merge(await context.CallActivityAsync<BpmnWorkflowState>(nameof({className}), state));";
        }
    }

    private MethodResult GenerateUserTask(UserTask userTask, TimeSpan? timeout)
    {
        return new(GenerateWaitForExternalEvent($"{userTask.Id}Completed", timeout), true, true);
    }

    private MethodResult GenerateReceiveTask(GenerateWorkflowContext ctx, ReceiveTask receiveTask, TimeSpan? timeout)
    {
        var messageName = ctx.Definitions.RootElements.OfType<Message>().FirstOrDefault(x => x.Id == $"{receiveTask.MessageRef}")?.Name ?? $"{receiveTask.Id}Completed";
        return new(GenerateWaitForExternalEvent(messageName, timeout), true, true);
    }

    private MethodResult GenerateServiceTask(GenerateWorkflowContext ctx, ServiceTask serviceTask, TimeSpan? timeout)
    {
        var className = GenerateActivityClass(ctx, serviceTask);
        return new(GenerateCallActivity(className, timeout), true, true);
    }

    private MethodResult GenerateEndEvent()
    {
        return new("return Task.FromResult(Array.Empty<CallHandlerAsync>());", false, false);
    }

    private MethodResult GenerateStartEvent() => new("", false, true);
    
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
                {{(i > 0 ? "else " : "")}}if ({{conditionFlows[i].ConditionExpression.Text.FirstOrDefault()}})
                {
                    return Task.FromResult<CallHandlerAsync[]>(new CallHandlerAsync[] { Call{{target.Id}}Async });
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
                        return Task.FromResult<CallHandlerAsync[]>(new CallHandlerAsync[] { Call{{target.Id}}Async });
                    }
                    """);
            }
            else
            {
                sourceBuilder.AppendLine($$"""
                    return Task.FromResult<CallHandlerAsync[]>(new CallHandlerAsync[] { Call{{target.Id}}Async });
                    """);
            }
        }

        return new(sourceBuilder.ToString(), false, false);
    }

    private List<(BoundaryEvent Event, EventDefinition Definition)> GetBoundaryEventDefinitions(Process process, FlowNode flowNode, out TimeSpan? timeout)
    {
        var events = process.FlowElements.OfType<BoundaryEvent>().Where(x => x.AttachedToRef.ToString() == flowNode.Id);
        var supportedTypes = new List<Type> { typeof(TimerEventDefinition), typeof(ErrorEventDefinition) };

        var q = from e in events
                from d in e.EventDefinitions
                where supportedTypes.Contains(d.GetType())
                select (e, d);

        var q2 = from e in events
                 from r in e.EventDefinitionRefs
                 from d in process.FlowElements.OfType<EventDefinition>()
                 where d.Id == r.ToString() && supportedTypes.Contains(d.GetType())
                 select (e, d);
        
        var defs = q.Concat(q2)
                    .OrderBy(x => supportedTypes.IndexOf(x.d.GetType()))
                    .ToList();

        timeout = GetTimeSpanFromDefinition(defs.Select(x => x.d).OfType<TimerEventDefinition>().FirstOrDefault());

        return defs;
    }

    private TimeSpan? GetTimeSpanFromDefinition(TimerEventDefinition timerDefinition)
    {
        if(timerDefinition is null)
        {
            return null;
        }
        switch(timerDefinition.ItemElementName)
        {
            case ItemChoiceType.TimeDuration:
                return XmlConvert.ToTimeSpan(timerDefinition.Item.Text[0]);
            default:
                throw new DiagnosticException(ExpressionTypeNotSupported, null, timerDefinition.ItemElementName);
        }
    }

    private string GenerateOutgoingFlows(GenerateWorkflowContext ctx, BaseElement elem)
    {
        var outgoingFlows = ctx.Process.FlowElements.OfType<SequenceFlow>().Where(x => x.SourceRef == elem.Id);
        var targets = ctx.Process.FlowElements.Where(x => outgoingFlows.Any(y => x.Id == y.TargetRef)).Select(x => $"Call{x.Id}Async");
        return string.Join(", ", targets);
    }

    private string GenerateActivityClass(GenerateWorkflowContext ctx, FlowElement activityClass) 
    {
        var id = activityClass.Id.Capatilize();
        var className = $"{id}Activity";
        
        if (!ctx.GeneratedClasses.Contains(className))
        {
            var src = $$"""
            using Dapr.Workflow;
            using Rex.Dapr.Workflow.Bpmn;

            namespace {{ctx.RootNamespace}}.Workflows.Activities
            {
                public partial class {{className}} : WorkflowActivity<BpmnWorkflowState, BpmnWorkflowState>
                {
                }
            }
            """;
            ctx.GeneratorExecutionContext.AddSource($"{className}.g.cs", src);
            ctx.GeneratedClasses.Add(className);
        }
        return className;
    }

    private void GenerateWorkflowExtensionClass(GenerateWorkflowContext ctx)
    {
        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine($$"""
            using {{ctx.RootNamespace}}.Workflows;
            using {{ctx.RootNamespace}}.Workflows.Activities;
            using Dapr.Workflow;

            namespace Microsoft.Extensions.DependencyInjection
            {
                public static class {{ctx.WorkflowClassName}}ServiceCollectionExtensions
                {
                    public static IServiceCollection Add{{ctx.WorkflowClassName}}(this IServiceCollection services)
                    {
                        services.AddDaprWorkflow(options =>
                        {
                            options.RegisterWorkflow<{{ctx.WorkflowClassName}}>();
                            options.RegisterActivity<{{ctx.WorkflowClassName}}LogActivity>();
            """);
        foreach (var className in ctx.GeneratedClasses.Where(x => x.EndsWith("Activity")))
        {
            sourceBuilder.AppendLine($$"""
                                options.RegisterActivity<{{className}}>();
                """);
        }
        sourceBuilder.AppendLine($$"""
                        });
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
            using Rex.Dapr.Workflow.Bpmn;
            using {{ctx.RootNamespace}}.Workflows;
            using Dapr.Workflow;
            using Microsoft.AspNetCore.Mvc;
            using System;
            using System.Threading.Tasks;

            namespace {{ctx.RootNamespace}}.Controllers
            {
                [ApiController]
                [Route("[controller]")]
                public partial class {{controllerClassName}} : ControllerBase
                {
                    private readonly ILogger<{{controllerClassName}}> _logger;
                    private readonly DaprWorkflowClient _workflowClient;

                    public {{controllerClassName}}(ILogger<{{controllerClassName}}> logger, DaprWorkflowClient workflowClient)
                    {
                        _logger = logger;
                        _workflowClient = workflowClient;
                    }

                    [HttpPost("")]
                    public async Task<string> NewWorkflowAsync(BpmnWorkflowState state)
                    {
                        var instanceId = Guid.NewGuid().ToString("N");
                        await _workflowClient.ScheduleNewWorkflowAsync(
                            name: nameof({{ctx.WorkflowClassName}}),
                            instanceId: instanceId,
                            input: state);
                        return instanceId;
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
                sourceBuilder.AppendLine($$"""
                            [HttpPost("{instanceId}/Raise{{messageName}}")]
                            public async Task<IActionResult> Raise{{messageName}}Async(string instanceId, [FromBody] BpmnWorkflowState state)
                            {
                                await _workflowClient.RaiseEventAsync(
                                    instanceId: instanceId,
                                    eventName: "{{messageName}}",
                                    eventPayload: state);
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
        ctx.GeneratorExecutionContext.AddSource("LogActivity.g.cs", src);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Nothing to initialize
    }
}
