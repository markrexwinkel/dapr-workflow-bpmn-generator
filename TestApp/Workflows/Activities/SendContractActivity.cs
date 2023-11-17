using Dapr.Workflow;
using Rex.Dapr.Workflow.Bpmn;

namespace TestApp.Workflows.Activities;

partial class SendContractActivity
{
    private readonly ILogger<SendContractActivity> _logger;

    public SendContractActivity(ILogger<SendContractActivity> logger)
    {
        _logger = logger;
    }

    public override Task<BpmnWorkflowState> RunAsync(WorkflowActivityContext context, BpmnWorkflowState input)
    {
        _logger.LogInformation($"{nameof(SendContractActivity)}.RunAsync called");
        return Task.FromResult(input);
    }
}
