using Dapr.Workflow;
using Rex.Dapr.Workflow.Bpmn;

namespace TestApp.Workflows.Activities;

partial class SendProposalActivity
{
    private readonly ILogger<SendProposalActivity> _logger;

    public SendProposalActivity(ILogger<SendProposalActivity> logger)
    {
        _logger = logger;
    }

    public override Task<BpmnWorkflowState> RunAsync(WorkflowActivityContext context, BpmnWorkflowState input)
    {
        _logger.LogInformation($"{nameof(SendProposalActivity)}.RunAsync called");
        return Task.FromResult(input);
    }
}
