using Microsoft.Extensions.DependencyInjection.Extensions;
using Rex.Bpmn.Dapr.Workflow;
using Rex.Bpmn.Dapr.Workflow.Activities;
using Rex.Bpmn.Dapr.Workflow.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IBpmnWorkflowBuilder AddBpmnWorkflow(this IServiceCollection services, Action<BpmnWorkflowOptions> configure = null)
    {
        var builder = new BpmnWorkflowBuilder(services);
        return builder.AddCoreServices(configure);
    }

    private static IBpmnWorkflowBuilder AddCoreServices(this IBpmnWorkflowBuilder builder, Action<BpmnWorkflowOptions> configure = null)
    {
        builder.Services.Configure(configure ?? Noop);
        builder.Services.TryAddScoped<IWorkflowService, BpmnWorkflowService>();
        return builder
            .TryRegisterActivity<StartBpmnWorkflowActivity>()
            .TryRegisterActivity<EndBpmnWorkflowActivity>()
            .TryRegisterActivity<EnterBpmnActivity>()
            .TryRegisterActivity<LeaveBpmnActivity>();

       static void Noop(BpmnWorkflowOptions options) { }
    }

}