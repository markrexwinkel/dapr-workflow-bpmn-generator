using global::Dapr.Workflow;
using Rex.Bpmn.Dapr.Workflow.Services;

namespace Rex.Bpmn.Dapr.Workflow.Activities;


public class StartBpmnWorkflowActivity(IWorkflowService workflowService) : WorkflowActivity<BpmnWorkflowInfo, object>
{
    private readonly IWorkflowService _workflowService = workflowService;

    public override async Task<object> RunAsync(WorkflowActivityContext context, BpmnWorkflowInfo state)
    {
        await _workflowService.StartWorkflowAsync(context.InstanceId, state);
        return null;
    }
}