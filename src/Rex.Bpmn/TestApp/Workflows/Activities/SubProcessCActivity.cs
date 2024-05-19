using Dapr.Workflow;

namespace TestApp.Workflows.Activities
{
    partial class SubProcessCActivity(ILogger<SubProcessCActivity> logger)
    {
        private readonly ILogger<SubProcessCActivity> _logger = logger;

        public override Task<object> RunAsync(WorkflowActivityContext context, SubProcessWorkflowState input)
        {
            _logger.LogInformation("[Workflow {InstanceId}] - Activity C executed.", context.InstanceId);
            return Task.FromResult<object>(null);
        }
    }
}
