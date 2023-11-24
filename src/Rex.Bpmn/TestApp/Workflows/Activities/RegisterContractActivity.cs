using Dapr.Workflow;

namespace TestApp.Workflows.Activities;

partial class RegisterContractActivity
{
    private readonly ILogger<RegisterContractActivity> _logger;

    public RegisterContractActivity(ILogger<RegisterContractActivity> logger)
    {
        _logger = logger;
    }

    public override Task<object> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
    {
        _logger.LogInformation($"[Workflow {context.InstanceId}] - Contract was registered.");
        return Task.FromResult<object>(null);
    }
}
