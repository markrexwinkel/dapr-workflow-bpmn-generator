using Dapr.Workflow;
using TestApp.Models;

namespace TestApp.Workflows.Activities;

partial class DetermineRiskProfileActivity(ILogger<DetermineRiskProfileActivity> logger)
{
    private readonly ILogger<DetermineRiskProfileActivity> _logger = logger;

    public override Task<RiskProfile> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
    {
        var riskClauses = new List<RiskClause>();

        int riskClass;
        if (input.LoanApplication.LoanAmount <= 10000)
        {
            riskClass = 1;
        }
        else if (input.LoanApplication.LoanAmount > 10000 && input.LoanApplication.LoanAmount <= 50000)
        {
            riskClass = 2;
            riskClauses.Add(RiskClause.A);
        }
        else if (input.LoanApplication.LoanAmount > 50000 && input.LoanApplication.LoanAmount <= 100000)
        {
            riskClass = 3;
            riskClauses.Add(RiskClause.A);
        }
        else
        {
            riskClass = 4;
            riskClauses.Add(RiskClause.A);
        }

        if (input.CustomerInfo.OutstandingAmount > 100000)
        {
            riskClauses.Add(RiskClause.B);
        }

        if (input.CustomerInfo.HasDefaulted == true)
        {
            riskClass += 1;
            riskClauses.Add(RiskClause.C);
        }

        var riskProfile = new RiskProfile { RiskClass = riskClass, RiskClauses = [.. riskClauses] };

        _logger.LogInformation("[Workflow {InstanceId}] - Risk profile was determined: {RiskProfile}.", context.InstanceId, riskProfile.Print());

        return Task.FromResult(riskProfile);
    }
}
