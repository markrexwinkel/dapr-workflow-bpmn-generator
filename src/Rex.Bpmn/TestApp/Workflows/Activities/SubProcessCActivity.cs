using Dapr.Workflow;

namespace TestApp.Workflows.Activities
{
    partial class SubProcessCActivity
    {
        private readonly ILogger<SubProcessCActivity> _logger;

        public SubProcessCActivity(ILogger<SubProcessCActivity> logger)
        {
            _logger = logger;
        }

        public override Task<object> RunAsync(WorkflowActivityContext context, SubProcessWorkflowState input)
        {
            _logger.LogInformation($"[Workflow {context.InstanceId}] - Activity C executed.");
            return Task.FromResult<object>(null);
        }
    }
}
