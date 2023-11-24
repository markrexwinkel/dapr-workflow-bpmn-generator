using Dapr.Workflow;

namespace TestApp.Workflows.Activities;

partial class SendRejectionLetterActivity
{
    private readonly ILogger<SendRejectionLetterActivity> _logger;

    public SendRejectionLetterActivity(ILogger<SendRejectionLetterActivity> logger)
    {
        _logger = logger;
    }

    public override Task<object> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
    {
        _logger.LogInformation($"[Workflow {context.InstanceId}] - Rejection letter was sent.");
        return Task.FromResult<object>(null);
    }
}
