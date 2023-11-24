using Dapr.Workflow;

namespace TestApp.Workflows.Activities;

partial class SendContractActivity
{
    private readonly ILogger<SendContractActivity> _logger;

    public SendContractActivity(ILogger<SendContractActivity> logger)
    {
        _logger = logger;
    }

    public override Task<object> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
    {
        _logger.LogInformation($"[Workflow {context.InstanceId}] - Contract was sent.");
        return Task.FromResult<object>(null);
    }
}
