using Dapr.Workflow;
using Rex.Dapr.Workflow.Bpmn;

namespace TestApp.Workflows.Activities;

partial class RegisterProspectActivity
{
    private readonly ILogger<RegisterProspectActivity> _logger;

    public RegisterProspectActivity(ILogger<RegisterProspectActivity> logger)
    {
        _logger = logger;
    }

    public override Task<BpmnWorkflowState> RunAsync(WorkflowActivityContext context, BpmnWorkflowState input)
    {
        _logger.LogInformation($"{nameof(RegisterProspectActivity)}.RunAsync called");
        return Task.FromResult(input);
    }
}
