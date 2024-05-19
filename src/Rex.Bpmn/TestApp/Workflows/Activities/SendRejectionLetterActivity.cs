using Dapr.Workflow;

namespace TestApp.Workflows.Activities;

partial class SendRejectionLetterActivity(ILogger<SendRejectionLetterActivity> logger)
{
    private readonly ILogger<SendRejectionLetterActivity> _logger = logger;

    public override Task<object> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
    {
        _logger.LogInformation("[Workflow {InstanceId}] - Rejection letter was sent.", context.InstanceId);
        return Task.FromResult<object>(null);
    }
}
