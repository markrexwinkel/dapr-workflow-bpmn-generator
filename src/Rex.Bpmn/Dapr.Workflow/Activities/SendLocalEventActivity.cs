using global::Dapr.Workflow;

namespace Rex.Bpmn.Dapr.Workflow.Activities;

public class SendLocalEvent
{
    public string Message { get; set; }
    public object Input { get; set; }
}

public class SendLocalEventActivity : WorkflowActivity<SendLocalEvent, object>
{
    private readonly DaprWorkflowClient _client;

    public SendLocalEventActivity(DaprWorkflowClient workflowClient)
    {
        _client = workflowClient;
    }

    public override async Task<object> RunAsync(WorkflowActivityContext context, SendLocalEvent state)
    {
        await _client.RaiseEventAsync(context.InstanceId, state.Message, state.Input);
        return null;
    }
}