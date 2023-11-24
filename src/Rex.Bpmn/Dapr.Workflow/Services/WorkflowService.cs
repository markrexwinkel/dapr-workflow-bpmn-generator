using Dapr.Client;
using Microsoft.Extensions.Options;

namespace Rex.Bpmn.Dapr.Workflow.Services;

#pragma warning disable CS0618

public class BpmnWorkflowService(DaprClient client, IOptionsSnapshot<BpmnWorkflowOptions> options) : IWorkflowService
{
    private readonly DaprClient _client = client;
    private readonly BpmnWorkflowOptions _options = options.Value;
    private readonly string _ownerId = Guid.NewGuid().ToString();

    public async Task StartWorkflowAsync(string instanceId, BpmnWorkflowInfo info, CancellationToken cancellationToken = default)
    {
        var key = GetKey(info.Name);
        await RunWithLockAsync(key, async () =>
        {
            var state = await _client.GetStateAsync<BpmnWorkflowState>(_options.StoreName, key, ConsistencyMode.Strong, cancellationToken: cancellationToken);
            state ??= new BpmnWorkflowState { Info = info };
            state.Instances.Add(instanceId);
            await _client.SaveStateAsync(_options.StoreName, key, state, cancellationToken: cancellationToken);
        }, cancellationToken);
    }

    public async Task EndWorkflowAsync(string name, string instanceId, CancellationToken cancellationToken = default)
    {
        var key = GetKey(name);
        var instanceKey = GetKey(instanceId);
        await RunWithLockAsync(key, async () =>
        {
            var state = await _client.GetStateAsync<BpmnWorkflowState>(_options.StoreName, key, ConsistencyMode.Strong, cancellationToken: cancellationToken);
            var activityState = await _client.GetStateAsync<BpmnWorkflowActivityState>(_options.StoreName, instanceKey, ConsistencyMode.Strong, cancellationToken: cancellationToken);
            
            if (state is not null)
            {
                state.Instances.Remove(instanceId);

                if (state.Instances.Count > 0)
                {
                    await _client.SaveStateAsync(_options.StoreName, key, state, cancellationToken: cancellationToken);
                }
                else
                {
                    await _client.DeleteStateAsync(_options.StoreName, key, cancellationToken: cancellationToken);
                }
            }
            if (activityState is not null)
            {
                await _client.DeleteStateAsync(_options.StoreName, instanceKey, cancellationToken: cancellationToken);
            }
        }, cancellationToken);
    }

    public Task<BpmnWorkflowState> GetWorkflowsAsync(string name, CancellationToken cancellationToken = default)
    {
        var key = GetKey(name);
        return _client.GetStateAsync<BpmnWorkflowState>(_options.StoreName, key, cancellationToken: cancellationToken);
    }

    private static string GetKey(params string[] parts) => $"workflowinfo_{string.Join("_", parts)}".ToLowerInvariant();

    public Task EnterActivityAsync(string instanceId, string flowId, CancellationToken cancellationToken = default)
    {
        var key = GetKey(instanceId);
        flowId ??= string.Empty;
        return RunWithLockAsync(key, async () =>
        {
            var state = await _client.GetStateAsync<BpmnWorkflowActivityState>(_options.StoreName, key, ConsistencyMode.Strong, cancellationToken: cancellationToken);
            state ??= new();
            state.Tokens.TryGetValue(flowId, out var count);
            state.Tokens[flowId] = count + 1;
            await _client.SaveStateAsync(_options.StoreName, key, state, cancellationToken: cancellationToken);
        }, cancellationToken);
    }

    public Task LeaveActivityAsync(string instanceId, string flowId, CancellationToken cancellationToken = default)
    {
        var key = GetKey(instanceId);
        flowId ??= string.Empty;
        return RunWithLockAsync(key, async () =>
        {
            var state = await _client.GetStateAsync<BpmnWorkflowActivityState>(_options.StoreName, key, ConsistencyMode.Strong, cancellationToken: cancellationToken);
            state ??= new();
            state.Tokens.TryGetValue(flowId, out var count);
            state.Tokens[flowId] = Math.Min(count - 1, 0);
            await _client.SaveStateAsync(_options.StoreName, key, state, cancellationToken: cancellationToken);
        }, cancellationToken);
    }

    private async Task RunWithLockAsync(string resource, Func<Task> handler, CancellationToken cancellationToken = default)
    {
        TryLockResponse lockResponse = null;
        try
        {
            var timeOutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(_options.LockTimeout).Token);
            while (!timeOutCts.IsCancellationRequested)
            {
                lockResponse = await _client.Lock(_options.LockStoreName, resource, _ownerId, _options.LockExpiryInSeconds, timeOutCts.Token);
                if (lockResponse.Success)
                {
                    break;
                }
                await Task.Delay(100, timeOutCts.Token);
            }
            if (timeOutCts.IsCancellationRequested && !lockResponse.Success)
            {
                throw new TimeoutException($"Failed to acquire lock for resource {resource}");
            }

            await handler();
        }
        finally
        {
            if (lockResponse?.Success == true)
            {
                await _client.Unlock(_options.LockStoreName, resource, _ownerId, cancellationToken);
            }
        }
    }
}