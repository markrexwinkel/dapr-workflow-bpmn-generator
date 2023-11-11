using Microsoft.CodeAnalysis;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Rex.Bpmn.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

#pragma warning disable RS1036

namespace Rex.Dapr.Workflow.Bpmn;

[Generator]
public class BpmnWorkflowGenerator : ISourceGenerator
{
    private readonly XmlSerializer _bpmnSerializer = new(typeof(Definitions));

    public void Execute(GeneratorExecutionContext context)
    {
        context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.projectdir", out var projectDir);
        context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.rootnamespace", out var rootNamespace);

        var bpmnFiles = GetBpmnFiles(projectDir);
        
        foreach(var bpmnFile in bpmnFiles)
        {
            using (var fs = File.OpenRead(bpmnFile))
            {
                var definitions = (Definitions)_bpmnSerializer.Deserialize(fs);
                foreach (var process in definitions.RootElements.OfType<Process>())
                {
                    var ctx = new GenerateWorkflowContext
                    {
                        GeneratorExecutionContext = context,
                        RootNamespace = rootNamespace,
                        WorkflowClassName = $"{process.Id}Workflow",
                        WorkflowInputClassName = process.Id,
                        WorkflowOutputClassName = $"{process.Id}Result"
                    };
                    GenerateInputClass(ctx);
                    GenerateOutputClass(ctx);
                    GenerateWorkflow(ctx);
                }
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
    }

    private void GenerateWorkflow(GenerateWorkflowContext ctx)
    {
        
        
        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine("using System;");
        sourceBuilder.AppendLine("using Dapr.Workflow;");
        sourceBuilder.AppendLine();
        sourceBuilder.AppendLine($"namespace {ctx.RootNamespace}");
        sourceBuilder.AppendLine("{");
        sourceBuilder.AppendLine($"\tpublic class {ctx.WorkflowClassName} : Workflow<{ctx.WorkflowInputClassName}, {ctx.WorkflowOutputClassName}>");
        sourceBuilder.AppendLine("\t{");
        sourceBuilder.AppendLine($"\t\tpublic override async Task<{ctx.WorkflowOutputClassName}> RunAsync(WorkflowContext context, {ctx.WorkflowInputClassName} input)");
        sourceBuilder.AppendLine("\t\t{");

        var startElement = ctx.Process.GetFlowElement<StartEvent>();
        // get target elements, lets start with support for one :-)
        var target = startElement.GetTargets(ctx.Process).FirstOrDefault();
        GenerateElement(ctx, target.element, target.flow, sourceBuilder);
        sourceBuilder.AppendLine("\t\t\treturn null;");
        sourceBuilder.AppendLine("\t\t}");
        sourceBuilder.AppendLine("\t}"); // class
        sourceBuilder.AppendLine("}"); // namespace
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
        }
    }

    private void GenerateServiceTask(GenerateWorkflowContext ctx, ServiceTask serviceTask, StringBuilder sourceBuilder)
    {
        var camundaType = serviceTask.GetExtensionAttribute("type", Namespaces.CamundaBpmn) ?? "";
        switch(camundaType)
        {
            default:
                // no type specified, generate partial activity class
                break;
        }
    }

    private void GenerateOutputClass(GenerateWorkflowContext ctx)
    {
        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine();
        sourceBuilder.AppendLine($"namespace {ctx.RootNamespace}");
        sourceBuilder.AppendLine("{");
        sourceBuilder.AppendLine($"\tpublic partial class {ctx.WorkflowOutputClassName}");
        sourceBuilder.AppendLine("\t{");
        sourceBuilder.AppendLine("\t}");
        sourceBuilder.AppendLine("}");
        ctx.GeneratorExecutionContext.AddSource($"{ctx.WorkflowOutputClassName}.g.cs", sourceBuilder .ToString());
    }

    private void GenerateInputClass(GenerateWorkflowContext ctx)
    {
        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine();
        sourceBuilder.AppendLine($"namespace {ctx.RootNamespace}");
        sourceBuilder.AppendLine("{");
        sourceBuilder.AppendLine($"\tpublic partial class {ctx.WorkflowInputClassName}");
        sourceBuilder.AppendLine("\t{");
        sourceBuilder.AppendLine("\t}");
        sourceBuilder.AppendLine("}");
        ctx.GeneratorExecutionContext.AddSource($"{ctx.WorkflowInputClassName}.g.cs", sourceBuilder.ToString());
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Nothing to initialize
    }

    private IEnumerable<string> GetBpmnFiles(string projectDir)
    {
        var matcher = new Matcher();
        matcher.AddInclude("**/*.bpmn");
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
