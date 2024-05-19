using Dapr.Workflow;
using Rex.Bpmn.Dapr.Workflow.Activities;
using Rex.Bpmn.Dapr.Workflow.Services;
using System.Collections.Concurrent;

namespace Rex.Bpmn.Dapr.Workflow;

public abstract class BpmnWorkflow<TInput, TOutput, TWorkflowState, TWorkflow> : Workflow<TInput, TOutput> where TWorkflow : IBpmnXmlProvider
{
    protected class CallHandlerResult(CallHandlerAsync next, string flowId, string activityId)
    {
        public CallHandlerAsync Next { get; } = next;
        public string FlowId { get; } = flowId;
        public string ActivityId { get; } = activityId;
    }

    protected class CallHandlerContext(string flowId, string activityId, int entryId = 0)
    {
        public string FlowId { get; } = flowId;
        public int EntryId { get; } = entryId;
        public string ActivityId { get; } = activityId;
    }

    protected delegate Task<CallHandlerResult[]> CallHandlerAsync(WorkflowContext context, TWorkflowState state, CallHandlerContext callContext);

    private readonly ConcurrentDictionary<string, int> _entries = new();
    
    public override async Task<TOutput> RunAsync(WorkflowContext context, TInput input)
    {
        try
        {
            var info = new BpmnWorkflowInfo
            {
                Name = context.Name,
                Xml = TWorkflow.GetXml()
            };
            await context.CallActivityAsync(nameof(StartBpmnWorkflowActivity), info);
            return await RunInternalAsync(context, input);
        }
        finally
        {
            await context.CallActivityAsync(nameof(EndBpmnWorkflowActivity), context.Name);
        }
    }

    protected abstract Task<TOutput> RunInternalAsync(WorkflowContext context, TInput input);

    protected async Task<CallHandlerResult[]> ExecuteActivityAsync(CallHandlerAsync handler, WorkflowContext context, TWorkflowState state, string flowId, string activityId)
    {
        try
        {
            var entryId = _entries.AddOrUpdate(activityId, 0, (_, v) => v + 1);
            var callContext = new CallHandlerContext(flowId, activityId, entryId);
            await context.CallActivityAsync(nameof(EnterBpmnActivity), callContext.FlowId);
            return await handler(context, state, callContext);
        }
        finally
        {
            _entries.TryRemove(activityId, out _);
            await context.CallActivityAsync(nameof(LeaveBpmnActivity), flowId);
        }
    }
}
