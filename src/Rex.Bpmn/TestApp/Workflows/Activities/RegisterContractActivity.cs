using Dapr.Workflow;

namespace TestApp.Workflows.Activities;

partial class RegisterContractActivity(ILogger<RegisterContractActivity> logger)
{
    private readonly ILogger<RegisterContractActivity> _logger = logger;

    public override Task<object> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
    {
        _logger.LogInformation("[Workflow {InstanceId}] - Contract was registered.", context.InstanceId);
        return Task.FromResult<object>(null);
    }
}
