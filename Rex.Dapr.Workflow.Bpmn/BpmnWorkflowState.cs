using System.Collections.Generic;

namespace Rex.Dapr.Workflow.Bpmn;

public class BpmnWorkflowState : Dictionary<string, object>
{
    public BpmnWorkflowState Merge(BpmnWorkflowState other)
    {
        if(other != this)
        {
            foreach(var kv in other)
            {
                this[kv.Key] = kv.Value;
            }
        }
        return this;
    }
}
