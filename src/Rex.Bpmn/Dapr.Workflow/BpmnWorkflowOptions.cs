namespace Rex.Bpmn.Dapr.Workflow;

public class BpmnWorkflowOptions
{
    public string StoreName { get; set; } = "statestore";
    public string LockStoreName { get; set; } = "lockstore";
    public int LockExpiryInSeconds { get; set; } = 5;
    public TimeSpan LockTimeout { get; set; } = TimeSpan.FromSeconds(5);
}