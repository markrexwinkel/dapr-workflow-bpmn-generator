using Dapr.Workflow;
using TestApp.Models;

namespace TestApp.Workflows.Activities;

partial class RegisterProspectActivity(ILogger<RegisterProspectActivity> logger)
{
    private readonly ILogger<RegisterProspectActivity> _logger = logger;

    public override Task<CustomerInfo> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
    {
        var customerInfo = new CustomerInfo(
            Id: Guid.NewGuid().ToString("D"),
            Name: input.LoanApplication.ApplicantName,
            OutstandingAmount: 0,
            HasDefaulted: false);

        _logger.LogInformation("[Workflow {InstanceId}] - New customer was registered.", context.InstanceId);

        return Task.FromResult(customerInfo);
    }
}
