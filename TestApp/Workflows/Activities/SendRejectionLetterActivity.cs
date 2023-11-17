using Dapr.Workflow;
using Rex.Dapr.Workflow.Bpmn;

namespace TestApp.Workflows.Activities;

partial class SendRejectionLetterActivity
{
    private readonly ILogger<SendRejectionLetterActivity> _logger;

    public SendRejectionLetterActivity(ILogger<SendRejectionLetterActivity> logger)
    {
        _logger = logger;
    }

    public override Task<BpmnWorkflowState> RunAsync(WorkflowActivityContext context, BpmnWorkflowState input)
    {
        _logger.LogInformation($"{nameof(SendRejectionLetterActivity)}.RunAsync called");
        return Task.FromResult(input);
    }
}
