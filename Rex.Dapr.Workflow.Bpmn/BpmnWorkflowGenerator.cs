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
public class BpmnWorkflowGenerator : ISourceGenerator
{
    private readonly XmlSerializer _bpmnSerializer = new(typeof(Definitions));
    private const string DiagnosticCategory = "Rex.Dapr.Workflow.Bpmn";
    private static readonly DiagnosticDescriptor FailedToLoadBpmn = new("BPMN0001", "Failed to load BPMN file", "Failed to load BPMN file {0}", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor NoStartEventFound = new("BPMN0002", "No start event found", "No start event found", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor MoreThanOneOutgoingWithoutCondition = new("BPMN0003", "More than one outgoing flow without condition", "There are more than one ({0}) outgoing flows without condition from element {1}, with condition: {2}, default: {3}", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor TargetDoesNotExist = new("BPMN0004", "Target not found", "Target {0} found for flow {1} and source {2} not found", DiagnosticCategory, DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor MoreThanOneOutgoingFlow = new("BPMN0005", "More than one outgoing flow", "Element {0} has more than one outgoing flow, which is not supported", DiagnosticCategory, DiagnosticSeverity.Warning, true);
    private static readonly DiagnosticDescriptor ExpressionTypeNotSupported = new("BPMN0006", "Expression type not supported", "Expression type {0} not supported", DiagnosticCategory, DiagnosticSeverity.Warning, true);

    public void Execute(GeneratorExecutionContext context)
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
                GenerateWorkflowExtensionClass(ctx);
                GenerateControllerClass(ctx);
            }
        }
    }

    public class GenerateWorkflowContext
    {
        public GeneratorExecutionContext GeneratorExecutionContext { get; set; }
        public string RootNamespace { get; set; }
        public string WorkflowClassName { get; set; }
        public Process Process { get; set; }
        public Definitions Definitions { get; set; }
        public HashSet<string> GeneratedClasses { get; private set; } = [];
        public int IndentLevel { get; set; }
        public string Indent => new string('\t', IndentLevel);
        
        public GenerateWorkflowContext IncIndent(int count = 1)
        {
            return new GenerateWorkflowContext
            {
                GeneratorExecutionContext = GeneratorExecutionContext,
                RootNamespace = RootNamespace,
                WorkflowClassName = WorkflowClassName,
                Definitions = Definitions,
                Process = Process,
                GeneratedClasses = GeneratedClasses,
                IndentLevel = IndentLevel + count
            };
        }
    }

    private void GenerateWorkflow(GenerateWorkflowContext ctx)
    {
        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine($$"""
            using {{ctx.RootNamespace}}.Workflows.Activities;
            using Dapr.Workflow;
            using Rex.Dapr.Workflow.Bpmn;
            using System;
            using System.Collections.Generic;
           
            namespace {{ctx.RootNamespace}}.Workflows
            {
                public class {{ctx.WorkflowClassName}} : Workflow<BpmnWorkflowState, BpmnWorkflowState>
                {
                    public override async Task<BpmnWorkflowState> RunAsync(WorkflowContext context, BpmnWorkflowState state)
                    {
            """);

        var startElement = ctx.Process.GetFlowElement<StartEvent>();
        if (startElement is null)
        {
            ctx.GeneratorExecutionContext.ReportDiagnostic(Diagnostic.Create(NoStartEventFound, null));
        }
        // get target elements, lets start with support for one :-)
        var (element, _) = startElement.GetTargets(ctx.Process).FirstOrDefault();
        GenerateElementCall(ctx.IncIndent(3), element, sourceBuilder);

        sourceBuilder.AppendLine("""
                        return state;
                    }
            """);

        foreach (var elem in ctx.Process.FlowElements.OfType<FlowNode>())
        {
            sourceBuilder.AppendLine();
            GenerateElement(ctx.IncIndent(2), elem, sourceBuilder);
        }

        sourceBuilder.AppendLine("""
                }
            }
            """);
        ctx.GeneratedClasses.Add(ctx.WorkflowClassName);
        ctx.GeneratorExecutionContext.AddSource($"{ctx.WorkflowClassName}.g.cs", sourceBuilder.ToString());
    }

    private void GenerateElementCall(GenerateWorkflowContext ctx, FlowElement elem, StringBuilder sourceBuilder)
    {
        sourceBuilder.AppendLine($$"""
            {{ctx.Indent}}state.Merge(await Call{{elem.Id}}Async(context, state));
            """);
    }

    private void GenerateElement(GenerateWorkflowContext ctx, FlowElement elem, StringBuilder sourceBuilder)
    {
        //TODO:
        switch(elem)
        {
            case ServiceTask serviceTask:
                GenerateServiceTask(ctx, serviceTask, sourceBuilder);
                break;
            case ExclusiveGateway exclusiveGateway:
                GenerateExclusiveGateway(ctx, exclusiveGateway, sourceBuilder);
                break;
            case EndEvent endEvent:
                GenerateEndEvent(ctx, endEvent, sourceBuilder);
                break;
            case UserTask userTask:
                GenerateUserTask(ctx, userTask, sourceBuilder);
                break;
            case ReceiveTask receiveTask:
                GenerateReceiveTask(ctx, receiveTask, sourceBuilder);
                break;
            case SendTask sendTask:
                GenerateSendTask(ctx, sendTask, sourceBuilder);
                break;
        }
    }

    private void GenerateFlowNodeMethodSignature(GenerateWorkflowContext ctx, FlowNode flowNode, StringBuilder sourceBuilder, bool isAsync = true)
    {
        sourceBuilder.AppendLine($$"""
            {{ctx.Indent}}private {{(isAsync ? "async " : "")}}Task<BpmnWorkflowState> Call{{flowNode.Id}}Async(WorkflowContext context, BpmnWorkflowState state)
            """);
    }

    private void GenerateUserTask(GenerateWorkflowContext ctx, UserTask userTask, StringBuilder sourceBuilder)
    {
        GenerateFlowNodeMethodSignature(ctx, userTask, sourceBuilder);
        var ctx2 = ctx.IncIndent();
        sourceBuilder.AppendLine($$"""
            {{ctx.Indent}}{
            {{ctx2.Indent}}state.Merge(await context.WaitForExternalEventAsync<BpmnWorkflowState>("{{userTask.Id}}Completed"));
            """);
        GenerateOutgoingFlows(ctx2, userTask, sourceBuilder);
        sourceBuilder.AppendLine($$"""
            {{ctx2.Indent}}return state;
            {{ctx.Indent}}}
            """);
    }

    private void GenerateReceiveTask(GenerateWorkflowContext ctx, ReceiveTask receiveTask, StringBuilder sourceBuilder)
    {
        var messageName = ctx.Definitions.RootElements.OfType<Message>().FirstOrDefault(x => x.Id == $"{receiveTask.MessageRef}")?.Name ?? "{{receiveTask.Id}}Completed";
        GenerateFlowNodeMethodSignature(ctx, receiveTask, sourceBuilder);
        sourceBuilder.AppendLine($$"""
            {{ctx.Indent}}{
            """);
        GenerateBoundaryEventDefinitions(ctx.IncIndent(), receiveTask, sourceBuilder, (ctx2, elem, sb, ts) =>
        {
            sb.AppendLine($$"""
                {{ctx2.Indent}}state.Merge(await context.WaitForExternalEventAsync<BpmnWorkflowState>("{{messageName}}"{{ (ts.HasValue ? $", TimeSpan.FromTicks({ts.Value.Ticks})" : "") }}));
                """);
            GenerateOutgoingFlows(ctx2, receiveTask, sourceBuilder);
            sb.AppendLine($$"""
                {{ctx2.Indent}}return state;
                """);
        });
        sourceBuilder.AppendLine($$"""
            {{ctx.Indent}}}
            """);
    }

    private void GenerateBoundaryEventDefinitions<T>(GenerateWorkflowContext ctx, T flowNode, StringBuilder sourceBuilder, Action<GenerateWorkflowContext, T, StringBuilder, TimeSpan?> inner) where T : FlowNode
    {
        var events = ctx.Process.FlowElements.OfType<BoundaryEvent>().Where(x => x.AttachedToRef.ToString() == flowNode.Id);
        var supportedTypes = new List<Type> { typeof(TimerEventDefinition), typeof(ErrorEventDefinition) };

        var q = from e in events
                from d in e.EventDefinitions
                where supportedTypes.Contains(d.GetType())
                select new { Event = e, Definition = d };

        var q2 = from e in events
                 from r in e.EventDefinitionRefs
                 from d in ctx.Process.FlowElements.OfType<EventDefinition>()
                 where d.Id == r.ToString() && supportedTypes.Contains(d.GetType())
                 select new { Event = e, Definition = d };

        var defs = q.Concat(q2)
                    .OrderBy(x => supportedTypes.IndexOf(x.Definition.GetType()))
                    .ToList();

        if (defs.Count > 0)
        {
            var ts = GetTimeSpanFromDefinition(ctx, defs.Select(x => x.Definition).OfType<TimerEventDefinition>().FirstOrDefault());
            sourceBuilder.AppendLine($$"""
                {{ctx.Indent}}try
                {{ctx.Indent}}{
                """);
            inner(ctx.IncIndent(), flowNode, sourceBuilder, ts);
            sourceBuilder.AppendLine($$"""
                {{ctx.Indent}}}
                """);
            foreach(var def in defs)
            {
                switch(def.Definition)
                {
                    case TimerEventDefinition timerDefinition:

                        sourceBuilder.AppendLine($$"""
                            {{ctx.Indent}}catch(TaskCanceledException)
                            {{ctx.Indent}}{
                            """);
                        GenerateOutgoingFlows(ctx.IncIndent(), def.Event, sourceBuilder);
                        sourceBuilder.AppendLine($$"""
                            {{ctx.Indent}}    return state;
                            {{ctx.Indent}}}
                            """);
                        break;
                    case ErrorEventDefinition errorEventDefinition:
                        sourceBuilder.AppendLine($$"""
                            {{ctx.Indent}}catch
                            {{ctx.Indent}}{
                        """);
                        GenerateOutgoingFlows(ctx.IncIndent(), def.Event, sourceBuilder);
                        sourceBuilder.AppendLine($$"""
                            {{ctx.Indent}}    return state;
                            {{ctx.Indent}}}
                            """);
                        break;
                }
            }
        }
        else
        {
            inner(ctx, flowNode, sourceBuilder, null);
        }
    }

    private TimeSpan? GetTimeSpanFromDefinition(GenerateWorkflowContext ctx, TimerEventDefinition timerDefinition)
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
                ctx.GeneratorExecutionContext.ReportDiagnostic(Diagnostic.Create(ExpressionTypeNotSupported, null, timerDefinition.ItemElementName));
                return null;
        }
    }

    private void GenerateSendTask(GenerateWorkflowContext ctx, SendTask sendTask, StringBuilder sourceBuilder)
    {

    }

    private void GenerateEndEvent(GenerateWorkflowContext ctx, EndEvent endEvent, StringBuilder sourceBuilder)
    {
        GenerateFlowNodeMethodSignature(ctx, endEvent, sourceBuilder, false);
        sourceBuilder.AppendLine($$"""
            {{ctx.Indent}}{
            {{ctx.Indent}}    return Task.FromResult(state);
            {{ctx.Indent}}}
            """);
    }

    private void GenerateExclusiveGateway(GenerateWorkflowContext ctx, ExclusiveGateway exclusiveGateway, StringBuilder sourceBuilder)
    {
        var outgoingFlows = ctx.Process.FlowElements.OfType<SequenceFlow>().Where(x => exclusiveGateway.Outgoing.Any(y => y.ToString() == x.Id)).ToList();
        var defaultFlow = outgoingFlows.FirstOrDefault(x => x.Id == exclusiveGateway.Default);
        var conditionFlows = outgoingFlows.Where(x => x.ConditionExpression != null).ToList();
        var nonConditionFlows = outgoingFlows.Except(conditionFlows).ToList();
        
        if ((outgoingFlows.Count > 1 && conditionFlows.Count == 0) || (defaultFlow is not null && nonConditionFlows.Count > 1)) 
        {
            ctx.GeneratorExecutionContext.ReportDiagnostic(Diagnostic.Create(MoreThanOneOutgoingWithoutCondition, null, outgoingFlows.Count, exclusiveGateway.Id, conditionFlows.Count, defaultFlow?.Id ?? "<none>"));
            return;
        }
        GenerateFlowNodeMethodSignature(ctx, exclusiveGateway, sourceBuilder);
        sourceBuilder.AppendLine($$"""
            {{ctx.Indent}}{
            """);
        var ctx2 = ctx.IncIndent();
        for(var i = 0; i < conditionFlows.Count; i++)
        {
            sourceBuilder.AppendLine($$"""
                {{ ctx2.Indent }}{{(i > 0 ? "else " : "")}}if ({{conditionFlows[i].ConditionExpression.Text.FirstOrDefault()}})
                {{ctx2.Indent}}{
                """);

            var target = ctx.Process.FlowElements.FirstOrDefault(x => x.Id == conditionFlows[i].TargetRef);
            if(target is null)
            {
                ctx.GeneratorExecutionContext.ReportDiagnostic(Diagnostic.Create(TargetDoesNotExist, null, conditionFlows[i].TargetRef, conditionFlows[i].Id, conditionFlows[i].SourceRef));
                return;
            }

            GenerateElementCall(ctx2.IncIndent(), target, sourceBuilder);

            sourceBuilder.AppendLine($$"""
                {{ ctx2.Indent }}    return state;
                {{ ctx2.Indent }}}
                """);
        }
        var lastFlow = defaultFlow ?? nonConditionFlows.FirstOrDefault();
        if(lastFlow is not null)
        {
            var indent = 0;
            if(conditionFlows.Count > 0)
            {
                indent++;
                sourceBuilder.AppendLine($$"""
                    {{ ctx2.Indent }}else
                    {{ ctx2.Indent }}{
                    """);
            }
            var target = ctx.Process.FlowElements.FirstOrDefault(x => x.Id == lastFlow.TargetRef);
            if (target is null)
            {
                ctx.GeneratorExecutionContext.ReportDiagnostic(Diagnostic.Create(TargetDoesNotExist, null, lastFlow.TargetRef, lastFlow.Id, lastFlow.SourceRef));
                return;
            }
            GenerateElementCall(ctx2.IncIndent(indent), target, sourceBuilder);
            if(conditionFlows.Count > 0)
            {
                sourceBuilder.AppendLine($$"""
                    {{ ctx2.Indent }}    return state;
                    {{ ctx2.Indent }}}
                    """);
            }
            else
            {
                sourceBuilder.AppendLine($$"""
                {{ctx.Indent}}    return state;
                """);
            }
        }

        sourceBuilder.AppendLine($$"""
            {{ctx.Indent}}}
            """);
    }

    private void GenerateOutgoingFlows(GenerateWorkflowContext ctx, BaseElement elem, StringBuilder sourceBuilder)
    {
        // Handle one outgoing flow for now
        var outgoingFlows = ctx.Process.FlowElements.OfType<SequenceFlow>().Where(x => x.SourceRef == elem.Id).ToList();
        if (outgoingFlows.Count > 1)
        {
            ctx.GeneratorExecutionContext.ReportDiagnostic(Diagnostic.Create(MoreThanOneOutgoingFlow, null, elem.Id));
        }
        if (outgoingFlows.Count > 0)
        {
            var target = ctx.Process.FlowElements.FirstOrDefault(x => x.Id == outgoingFlows[0].TargetRef);
            if (target is not null)
            {
                GenerateElementCall(ctx, target, sourceBuilder);
            }
            else
            {
                ctx.GeneratorExecutionContext.ReportDiagnostic(Diagnostic.Create(TargetDoesNotExist, null, outgoingFlows[0].TargetRef, outgoingFlows[0].Id, outgoingFlows[0].SourceRef));
            }
        }
    }

    private void GenerateServiceTask(GenerateWorkflowContext ctx, ServiceTask serviceTask, StringBuilder sourceBuilder)
    {
        var className = GenerateActivityClass(ctx, serviceTask);
        GenerateFlowNodeMethodSignature(ctx, serviceTask, sourceBuilder);
        var ctx2 = ctx.IncIndent();
        sourceBuilder.AppendLine($$"""
            {{ctx.Indent}}{
            {{ctx2.Indent}}state.Merge(await context.CallActivityAsync<BpmnWorkflowState>(nameof({{className}}), state));
            """);

        GenerateOutgoingFlows(ctx2, serviceTask, sourceBuilder);

        sourceBuilder.AppendLine($$"""
            {{ctx2.Indent}}return state;
            {{ctx.Indent}}}
            """);
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

    public void Initialize(GeneratorInitializationContext context)
    {
        // Nothing to initialize
    }
}
