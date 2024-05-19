using Dapr.Workflow;

namespace TestApp.Workflows.Activities;

partial class SendContractActivity(ILogger<SendContractActivity> logger)
{
    private readonly ILogger<SendContractActivity> _logger = logger;

    public override Task<object> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
    {
        _logger.LogInformation("[Workflow {InstanceId}] - Contract was sent.", context.InstanceId);
        return Task.FromResult<object>(null);
    }
}
