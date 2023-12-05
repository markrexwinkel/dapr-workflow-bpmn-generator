using Microsoft.Extensions.DependencyInjection;

namespace Rex.Bpmn.Dapr.Workflow;

public class BpmnWorkflowBuilder(IServiceCollection services) : IBpmnWorkflowBuilder
{
    public IServiceCollection Services { get; } = services;
}
