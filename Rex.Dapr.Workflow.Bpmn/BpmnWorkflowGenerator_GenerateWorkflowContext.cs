using Microsoft.CodeAnalysis;
using Rex.Bpmn.Model;
using Rex.Dapr.Workflow.Bpmn.Model;
using System.Collections.Generic;

namespace Rex.Dapr.Workflow.Bpmn;

partial class BpmnWorkflowGenerator
{
    public class GenerateWorkflowContext
    {
        public GeneratorExecutionContext GeneratorExecutionContext { get; set; }
        public string RootNamespace { get; set; }
        public string WorkflowClassName { get; set; }
        public string WorkflowStateClassName => $"{WorkflowClassName}State";
        public List<DaprParameter> WorkflowStateProperties { get; } = new();
        public Process Process { get; set; }
        public Definitions Definitions { get; set; }
        public HashSet<string> GeneratedClasses { get; private set; } = [];
        public string WorkflowInputType { get; set; }
        public string WorkflowOutputType { get; set; }
    }
}
