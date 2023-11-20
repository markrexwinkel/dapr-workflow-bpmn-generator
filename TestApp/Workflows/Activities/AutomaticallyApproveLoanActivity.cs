using Dapr.Workflow;

namespace TestApp.Workflows.Activities;

partial class AutomaticallyApproveLoanActivity
{
    public override Task<bool> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
    {
        return Task.FromResult(true);
    }
}
