using Dapr.Workflow;

namespace TestApp.Workflows.Activities;

partial class SendProposalActivity(ILogger<SendProposalActivity> logger)
{
    private readonly ILogger<SendProposalActivity> _logger = logger;

    public override Task<object> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
    {
        _logger.LogInformation("[Workflow {InstanceId}] - Loan proposal was sent.", context.InstanceId);
        return Task.FromResult<object>(null);
    }
}
