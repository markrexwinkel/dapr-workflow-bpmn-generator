using Dapr.Workflow;
using Rex.Dapr.Workflow.Bpmn;
using System;
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

    public override Task<BpmnWorkflowState> RunAsync(WorkflowActivityContext context, BpmnWorkflowState input)
    {
        _logger.LogInformation($"{nameof(DetermineExistingCustomerActivity)}.RunAsync called");
        foreach(var kv in input)
        {
            _logger.LogInformation($"{kv.Key}={kv.Value}");
        }
        var customerName = (string)input["ApplicantName"];
        if (customerName.Equals("john doe", StringComparison.InvariantCultureIgnoreCase))
        {
            return Task.FromResult(input);
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
        input["CustomerInfo"] = info;
        return Task.FromResult(input);
    }
}
