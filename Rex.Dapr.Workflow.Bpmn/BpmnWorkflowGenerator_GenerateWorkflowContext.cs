using Microsoft.CodeAnalysis;
using Rex.Bpmn.Model;
using System.Collections.Generic;

namespace Rex.Dapr.Workflow.Bpmn;

partial class BpmnWorkflowGenerator
{
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
}
