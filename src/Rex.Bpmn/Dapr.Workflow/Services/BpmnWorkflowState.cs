namespace Rex.Bpmn.Dapr.Workflow.Services;

public class BpmnWorkflowState
{
    public BpmnWorkflowInfo Info { get; set; }
    public List<string> Instances { get; set; } = [];
}
