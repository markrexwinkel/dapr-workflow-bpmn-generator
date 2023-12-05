using Microsoft.Extensions.DependencyInjection;

namespace Rex.Bpmn.Dapr.Workflow;

public interface IBpmnWorkflowBuilder
{
    IServiceCollection Services { get; }
}
