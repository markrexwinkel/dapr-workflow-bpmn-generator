using Dapr.Workflow;
using TestApp.Models;

namespace TestApp.Workflows.Activities;

partial class RegisterProspectActivity
{
    private readonly ILogger<RegisterProspectActivity> _logger;

    public RegisterProspectActivity(ILogger<RegisterProspectActivity> logger)
    {
        _logger = logger;
    }

    public override Task<CustomerInfo> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
    {
        var customerInfo = new CustomerInfo(
            Id: Guid.NewGuid().ToString("D"),
            Name: input.LoanApplication.ApplicantName,
            OutstandingAmount: 0,
            HasDefaulted: false);

        _logger.LogInformation($"[Workflow {context.InstanceId}] - New customer was registered.");

        return Task.FromResult<CustomerInfo>(customerInfo);
    }
}
