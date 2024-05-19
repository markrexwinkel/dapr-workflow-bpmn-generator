using Dapr.Workflow;
using Rex.Bpmn.Dapr.Workflow.Services;

namespace Rex.Bpmn.Dapr.Workflow.Activities;

public class EndBpmnWorkflowActivity(IWorkflowService workflowService) : WorkflowActivity<string, object>
{
    private readonly IWorkflowService _workflowService = workflowService;

    public override async Task<object> RunAsync(WorkflowActivityContext context, string workflowName)
    {
        await _workflowService.EndWorkflowAsync(workflowName, context.InstanceId);
        return null;
    }
}
