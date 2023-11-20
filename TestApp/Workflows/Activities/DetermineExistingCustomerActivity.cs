using Dapr.Workflow;
using System.Runtime.InteropServices;
using TestApp.Models;

namespace TestApp.Workflows.Activities;

partial class DetermineExistingCustomerActivity
{
    private readonly ILogger<DetermineExistingCustomerActivity> _logger;
    private readonly Random _random = new();

    public DetermineExistingCustomerActivity(ILogger<DetermineExistingCustomerActivity> logger)
    {
        _logger = logger;
    }

    public override Task<CustomerInfo> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
    {
        var customerName = input.LoanApplication.ApplicantName;
        if (customerName.Equals("john doe", StringComparison.InvariantCultureIgnoreCase))
        {
            return Task.FromResult<CustomerInfo>(null);
        }

        // Generate random demo customer info based on the input
        CustomerInfo info;
        if (customerName.Equals("eric white", StringComparison.InvariantCultureIgnoreCase))
        {
            info = new CustomerInfo(
                Id: Guid.NewGuid().ToString("D"),
                Name: customerName,
                OutstandingAmount: 12500,
                HasDefaulted: false);
        }
        else if (customerName.Equals("eric grey", StringComparison.InvariantCultureIgnoreCase))
        {
            info = new CustomerInfo(
                Id: Guid.NewGuid().ToString("D"),
                Name: customerName,
                OutstandingAmount: 105000,
                HasDefaulted: false);
        }
        else if (customerName.Equals("eric black", StringComparison.InvariantCultureIgnoreCase))
        {
            info = new CustomerInfo(
                Id: Guid.NewGuid().ToString("D"),
                Name: customerName,
                OutstandingAmount: 112000,
                HasDefaulted: true);
        }
        else
        {
            info = new CustomerInfo(
                Id: Guid.NewGuid().ToString("D"),
                Name: customerName,
                OutstandingAmount: _random.Next(10000, 100000),
                HasDefaulted: _random.Next(10) % 2 == 0);
        }
        return Task.FromResult(info);
    }
}
