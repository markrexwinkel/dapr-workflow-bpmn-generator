using Dapr.Workflow;
using Rex.Dapr.Workflow.Bpmn;

namespace TestApp.Workflows.Activities;

partial class DetermineRiskProfileActivity
{
    private readonly ILogger<DetermineRiskProfileActivity> _logger;

    public DetermineRiskProfileActivity(ILogger<DetermineRiskProfileActivity> logger)
    {
        _logger = logger;
    }

    public override Task<BpmnWorkflowState> RunAsync(WorkflowActivityContext context, BpmnWorkflowState input)
    {
        _logger.LogInformation($"{nameof(DetermineRiskProfileActivity)}.RunAsync called");
        return Task.FromResult(input);
    }
}
