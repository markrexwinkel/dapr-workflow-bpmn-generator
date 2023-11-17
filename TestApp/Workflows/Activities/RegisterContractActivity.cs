using Dapr.Workflow;
using Rex.Dapr.Workflow.Bpmn;

namespace TestApp.Workflows.Activities;

partial class RegisterContractActivity
{
    private readonly ILogger<RegisterContractActivity> _logger;

    public RegisterContractActivity(ILogger<RegisterContractActivity> logger)
    {
        _logger = logger;
    }

    public override Task<BpmnWorkflowState> RunAsync(WorkflowActivityContext context, BpmnWorkflowState input)
    {
        _logger.LogInformation($"{nameof(RegisterContractActivity)}.RunAsync called");
        return Task.FromResult(input);
    }
}
