using Dapr.Workflow;
using Rex.Bpmn.Dapr.Workflow.Activities;
using Rex.Bpmn.Dapr.Workflow.Services;

namespace Rex.Bpmn.Dapr.Workflow;

public abstract class BpmnWorkflow<TInput, TOutput, TWorkflowState> : Workflow<TInput, TOutput>
{
    protected class CallHandlerResult(CallHandlerAsync next, string flowId)
    {
        public CallHandlerAsync Next { get; } = next;
        public string FlowId { get; } = flowId;
    }

    protected delegate Task<CallHandlerResult[]> CallHandlerAsync(WorkflowContext context, TWorkflowState state, string flowId);

    public override async Task<TOutput> RunAsync(WorkflowContext context, TInput input)
    {
        try
        {
            var info = new BpmnWorkflowInfo
            {
                Name = context.Name,
                Xml = GetXml()
            };
            await context.CallActivityAsync(nameof(StartBpmnWorkflowActivity), info);
            return await RunInternalAsync(context, input);
        }
        finally
        {
            await context.CallActivityAsync(nameof(EndBpmnWorkflowActivity), context.Name);
        }
    }

    protected abstract string GetXml();

    protected abstract Task<TOutput> RunInternalAsync(WorkflowContext context, TInput input);

    protected async Task<CallHandlerResult[]> ExecuteActivityAsync(CallHandlerAsync handler, WorkflowContext context, TWorkflowState state, string flowId)
    {
        try
        {
            await context.CallActivityAsync(nameof(EnterBpmnActivity), flowId);
            return await handler(context, state, flowId);
        }
        finally
        {
            await context.CallActivityAsync(nameof(LeaveBpmnActivity), flowId);
        }
    }
}
