using Dapr.Workflow;
using Rex.Bpmn.Dapr.Workflow.Services;

namespace Rex.Bpmn.Dapr.Workflow.Activities;

public class EnterBpmnActivity(IWorkflowService workflowService) : WorkflowActivity<string, object>
{
    private readonly IWorkflowService _workflowService = workflowService;

    public override async Task<object> RunAsync(WorkflowActivityContext context, string input)
    {
        await _workflowService.EnterActivityAsync(context.InstanceId, input);
        return null;
    }
}
