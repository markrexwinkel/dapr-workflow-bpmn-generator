using Microsoft.CodeAnalysis;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Win32.SafeHandles;
using Rex.Bpmn.Model;
using Rex.Dapr.Workflow.Bpmn.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

#pragma warning disable RS1036

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
                    WorkflowInputClassName = process.Id,
                    WorkflowOutputClassName = $"{process.Id}Result",
                    Process = process
                };
                GenerateClass(ctx, ctx.WorkflowInputClassName, ctx.Process.GetDaprInputParameters());
                GenerateClass(ctx, ctx.WorkflowOutputClassName, ctx.Process.GetDaprOutputParameters());
                GenerateWorkflow(ctx);
            }
        }
    }

    public class GenerateWorkflowContext
    {
        public GeneratorExecutionContext GeneratorExecutionContext { get; set; }
        public string RootNamespace { get; set; }
        public string WorkflowClassName { get; set; }
        public string WorkflowInputClassName { get; set; }
        public string WorkflowOutputClassName { get; set; }
        public Process Process { get; set; }
        public HashSet<string> GeneratedClasses { get; }  = new();
    }

    private void GenerateWorkflow(GenerateWorkflowContext ctx)
    {
        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine($$"""
            using Dapr.Workflow;
            using System;
            using System.Collections.Generic;
           
            namespace {{ctx.RootNamespace}}
            {
                public class {{ctx.WorkflowClassName}} : Workflow<{{ctx.WorkflowInputClassName}}, {{ctx.WorkflowOutputClassName}}>
                {
                    public override async Task<{{ctx.WorkflowOutputClassName}}> RunAsync(WorkflowContext context, {{ctx.WorkflowInputClassName}} input)
                    {
                        var locals = new Dictionary<string, object>();
                        var output = new {{ctx.WorkflowOutputClassName}}();
            """);

        var startElement = ctx.Process.GetFlowElement<StartEvent>();
        if (startElement is null)
        {
            ctx.GeneratorExecutionContext.ReportDiagnostic(Diagnostic.Create(NoStartEventFound, null));
        }
        // get target elements, lets start with support for one :-)
        var (element, flow) = startElement.GetTargets(ctx.Process).FirstOrDefault();
        GenerateElement(ctx, element, flow, sourceBuilder);

        sourceBuilder.AppendLine("""
                        return output;
                    }
                }
            }
            """);

        ctx.GeneratorExecutionContext.AddSource($"{ctx.WorkflowClassName}.g.cs", sourceBuilder.ToString());
    }

    private void GenerateElement(GenerateWorkflowContext ctx, FlowElement elem, SequenceFlow flow, StringBuilder sourceBuilder)
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
        }
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
        
        for(var i = 0; i < conditionFlows.Count; i++)
        {
            sourceBuilder.AppendLine($$"""
                            {{(i > 0 ? "else " : "")}}if ({{conditionFlows[i].ConditionExpression.Text.FirstOrDefault()}})
                            {
                            """);
            var target = ctx.Process.FlowElements.FirstOrDefault(x => x.Id == conditionFlows[i].TargetRef);
            if(target is null)
            {
                ctx.GeneratorExecutionContext.ReportDiagnostic(Diagnostic.Create(TargetDoesNotExist, null, conditionFlows[i].TargetRef, conditionFlows[i].Id, conditionFlows[i].SourceRef));
                return;
            }
            GenerateElement(ctx, target, conditionFlows[i], sourceBuilder);
            sourceBuilder.AppendLine("""
                            }
                """);
        }
        var lastFlow = defaultFlow ?? nonConditionFlows.FirstOrDefault();
        if(lastFlow is not null)
        {
            if(conditionFlows.Count > 0)
            {
                sourceBuilder.AppendLine("""
                                else
                                {
                    """);
            }
            var target = ctx.Process.FlowElements.FirstOrDefault(x => x.Id == lastFlow.TargetRef);
            if (target is null)
            {
                ctx.GeneratorExecutionContext.ReportDiagnostic(Diagnostic.Create(TargetDoesNotExist, null, lastFlow.TargetRef, lastFlow.Id, lastFlow.SourceRef));
                return;
            }
            GenerateElement(ctx, target, lastFlow, sourceBuilder);
            if(conditionFlows.Count > 0)
            {
                sourceBuilder.AppendLine("""
                                }
                    """);
            }
        }
    }

    private void GenerateServiceTask(GenerateWorkflowContext ctx, ServiceTask serviceTask, StringBuilder sourceBuilder)
    {
        var camundaType = serviceTask.GetExtensionAttribute("type", Namespaces.CamundaBpmn) ?? "";
        switch(camundaType)
        {
            default:
                // no type specified, generate partial activity class
                (var className, var inputClassName, var outputClassName) = GenerateActivityClass(ctx, serviceTask);
                GenerateCallActivityMethod(serviceTask, className, inputClassName, outputClassName, sourceBuilder);
                break;
        }
        // Handle one outgoing flow for now
        var outgoingFlows = ctx.Process.FlowElements.OfType<SequenceFlow>().Where(x => serviceTask.Outgoing.Any(y => y.ToString() == x.Id)).ToList();
        if(outgoingFlows.Count > 1)
        {
            ctx.GeneratorExecutionContext.ReportDiagnostic(Diagnostic.Create(MoreThanOneOutgoingFlow, null, serviceTask.Id));
        }
        if(outgoingFlows.Count > 0)
        {
            var target = ctx.Process.FlowElements.FirstOrDefault(x => x.Id == outgoingFlows[0].TargetRef);
            if (target is not null)
            {
                GenerateElement(ctx, target, outgoingFlows[0], sourceBuilder);
            }
            else
            {
                ctx.GeneratorExecutionContext.ReportDiagnostic(Diagnostic.Create(TargetDoesNotExist, null, outgoingFlows[0].TargetRef, outgoingFlows[0].Id, outgoingFlows[0].SourceRef));
            }
        }
    }

    private void GenerateCallActivityMethod(FlowElement elem, string className, string inputClassName, string outputClassName, StringBuilder sourceBuilder)
    {
        var inputVar = inputClassName.Uncapatilize();
        var outputVar = outputClassName.Uncapatilize();
        sourceBuilder.AppendLine($$"""
                        {{ (string.IsNullOrEmpty(elem.Name) ? string.Empty : $"{Environment.NewLine}// {Regex.Replace(elem.Name, @"\s+", " ")}")}}
                        var {{inputVar}} = new {{inputClassName}}
                        {
            """);
        foreach(var parameter in elem.GetDaprInputParameters())
        {
            var lhs = parameter.Name;
            var rhs = parameter.Scope == DaprParameterScope.Local ? $"({parameter.Type}) locals[\"{parameter.Ref ?? parameter.Name}\"]" : $"input.{parameter.Ref}";
            sourceBuilder.AppendLine($$"""
                                {{lhs}} = {{rhs}},
                """);
        }
        sourceBuilder.AppendLine($$"""
                    };
                    var {{outputVar}} = await context.CallActivityAsync<{{outputClassName}}>(nameof({{className}}), {{inputVar}});
        """);
        foreach (var parameter in elem.GetDaprOutputParameters())
        {
            var rhs = $"{outputVar}.{parameter.Name}";
            var lhs = parameter.Scope == DaprParameterScope.Local ? $"locals[\"{parameter.Ref ?? parameter.Name}\"]" : $"input.{parameter.Ref}";
            sourceBuilder.AppendLine($$"""
                            {{lhs}} = {{rhs}};
                """);
        }
    }

    private (string className, string inputClassName, string outputClassName) GenerateActivityClass(GenerateWorkflowContext ctx, FlowElement activityClass) 
    {
        var id = activityClass.Id.Capatilize();
        var className = $"{id}Activity";
        var inputClassName = $"{id}Input";
        var outputClassName = $"{id}Output";
        if (!ctx.GeneratedClasses.Contains(className))
        {
            var src = $$"""
            using Dapr.Workflow;

            namespace {{ctx.RootNamespace}}
            {
                public partial class {{className}} : WorkflowActivity<{{inputClassName}}, {{outputClassName}}>
                {
                    public override async Task<{{outputClassName}}> RunAsync(WorkflowActivityContext context, {{inputClassName}} input)
                    {
                        var output = new {{outputClassName}}();
                        return output;
                    }
                }
            }
            """;
            ctx.GeneratorExecutionContext.AddSource($"{className}.g.cs", src);
            ctx.GeneratedClasses.Add(className);
        }
        GenerateClass(ctx, inputClassName, activityClass.GetDaprInputParameters());
        GenerateClass(ctx, outputClassName, activityClass.GetDaprOutputParameters());
        return (className, inputClassName, outputClassName);
    }

    private void GenerateClass(GenerateWorkflowContext ctx, string className, IEnumerable<DaprParameter> parameters)
    {
        if (!ctx.GeneratedClasses.Contains(className))
        {
            var sourceBuilder = new StringBuilder();
            sourceBuilder.AppendLine($$"""
            namespace {{ctx.RootNamespace}}
            {
                public partial class {{className}}
                {
            """);

            foreach (var parameter in parameters)
            {
                sourceBuilder.AppendLine($$"""
                        public {{parameter.Type}} {{parameter.Name}} { get; set; }
                """);
            }

            sourceBuilder.AppendLine("""
                }
            }
            """);
            ctx.GeneratorExecutionContext.AddSource($"{className}.g.cs", sourceBuilder.ToString());
            ctx.GeneratedClasses.Add(className);
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Nothing to initialize
    }

    private IEnumerable<string> GetBpmnFiles(string projectDir)
    {
        var matcher = new Matcher();
        matcher.AddInclude("**/*.bpmn");
        matcher.AddExclude("bin/**");
        matcher.AddExclude("obj/**");
        var result = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(projectDir)));
        if (result.HasMatches)
        {
            foreach (var file in result.Files)
            {
                yield return Path.Combine(projectDir, file.Path);
            }
        }
    }
}
