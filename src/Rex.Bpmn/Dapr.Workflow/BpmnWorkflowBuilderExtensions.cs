using Dapr.Workflow;
using Google.Api;
using Rex.Bpmn.Dapr.Workflow;

namespace Microsoft.Extensions.DependencyInjection;

public static class BpmnWorkflowBuilderExtensions
{
    private class IBpmnWorkflowRegistrar<TWorkflowOrActivity> { }

    public static IBpmnWorkflowBuilder TryRegisterWorkflow<TWorkflow>(this IBpmnWorkflowBuilder builder) where TWorkflow : class, IWorkflow, new()
    {
        var exists = builder.Services.FirstOrDefault(x => x.ServiceType == typeof(IBpmnWorkflowRegistrar<TWorkflow>)) is not null;
        if (!exists)
        {
            builder.Services.AddSingleton<IBpmnWorkflowRegistrar<TWorkflow>>(_ => null);
            builder.Services.AddDaprWorkflow(options => options.RegisterWorkflow<TWorkflow>());
        }
        return builder;
    }

    public static IBpmnWorkflowBuilder TryRegisterActivity<TActivity>(this IBpmnWorkflowBuilder builder) where TActivity : class, IWorkflowActivity
    {
        var exists = builder.Services.FirstOrDefault(x => x.ServiceType == typeof(IBpmnWorkflowRegistrar<TActivity>)) is not null;
        if (!exists)
        {
            builder.Services.AddSingleton<IBpmnWorkflowRegistrar<TActivity>>(_ => null);
            builder.Services.AddDaprWorkflow(options => options.RegisterActivity<TActivity>());
        }
        return builder;
    }
}
