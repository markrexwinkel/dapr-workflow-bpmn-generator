using Dapr.Workflow;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rex.Bpmn.Dapr.Workflow;
using Rex.Bpmn.Dapr.Workflow.Activities;
using Rex.Bpmn.Dapr.Workflow.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBpmnWorkflows(this IServiceCollection services, Action<BpmnWorkflowOptions> configure = null)
    {
        if (configure is not null)
        {
            services.Configure(configure);
        }
        services.TryAddScoped<IWorkflowService, BpmnWorkflowService>();
        services.AddDaprWorkflow(options =>
        {
            options.RegisterActivity<StartBpmnWorkflowActivity>();
            options.RegisterActivity<EndBpmnWorkflowActivity>();
            options.RegisterActivity<EnterBpmnActivity>();
            options.RegisterActivity<LeaveBpmnActivity>();
                
        });
        return services;
    }
}