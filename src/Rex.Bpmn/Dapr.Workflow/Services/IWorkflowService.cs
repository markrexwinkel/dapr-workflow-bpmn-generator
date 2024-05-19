namespace Rex.Bpmn.Dapr.Workflow.Services;

public interface IWorkflowService
{
    Task StartWorkflowAsync(string instanceId, BpmnWorkflowInfo info, CancellationToken cancellationToken = default);
    Task EndWorkflowAsync(string name, string instanceId, CancellationToken cancellationToken = default);
    Task EnterActivityAsync(string instanceId, string flowId, CancellationToken cancellationToken= default);
    Task LeaveActivityAsync(string instanceId, string flowId, CancellationToken cancellationToken = default);
    Task<BpmnWorkflowActivityState> GetActivityStateAsync(string instanceId, CancellationToken cancellationToken = default);
}