using Dapr.Workflow;

namespace TestApp.Workflows.Activities;

partial class SendProposalActivity
{
    private readonly ILogger<SendProposalActivity> _logger;

    public SendProposalActivity(ILogger<SendProposalActivity> logger)
    {
        _logger = logger;
    }

    public override Task<object> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
    {
        _logger.LogInformation($"[Workflow {context.InstanceId}] - Loan proposal was sent.");
        return Task.FromResult<object>(null);
    }
}
