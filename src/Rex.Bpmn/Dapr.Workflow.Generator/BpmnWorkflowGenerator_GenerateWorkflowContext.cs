using Microsoft.CodeAnalysis;
using Rex.Bpmn.Abstractions.Model;
using System.Collections.Generic;

namespace Rex.Bpmn.Dapr.Workflow.Generator;

partial class BpmnWorkflowGenerator
{
    public class GenerateWorkflowContext
    {
        public GeneratorExecutionContext GeneratorExecutionContext { get; set; }
        public string RootNamespace { get; set; }
        public string WorkflowClassName => $"{Process.Id}Workflow";
        public string WorkflowStateClassName => $"{WorkflowClassName}State";
        public List<DaprParameter> WorkflowStateProperties { get; } = new();
        public IFlowElements Process { get; set; }
        public Definitions Definitions { get; set; }
        public HashSet<string> WorkflowClasses { get; private set; } = [];
        public HashSet<string> ActivityClasses { get; private set; } = [];
        public string WorkflowInputType { get; set; }
        public string WorkflowOutputType { get; set; }
        public DaprParameter WorkflowInputParameter { get; set; }
        public DaprParameter WorkflowOutputParameter { get; set; }
        public string Xml { get; set; }


        public GenerateWorkflowContext ForSubProcess(IFlowElements flowElements)
        {
            var ret = new GenerateWorkflowContext
            {
                Definitions = Definitions,
                RootNamespace = RootNamespace,
                GeneratorExecutionContext = GeneratorExecutionContext,
                WorkflowClasses = WorkflowClasses,
                ActivityClasses = ActivityClasses,
                Process = flowElements,
                Xml = Xml,
            };
            WorkflowClasses.Add(ret.WorkflowClassName);
            return ret;
        }
    }
}
