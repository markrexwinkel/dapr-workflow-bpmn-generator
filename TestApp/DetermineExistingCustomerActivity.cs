﻿using Dapr.Workflow;
using Rex.Dapr.Workflow.Bpmn;

namespace TestApp;

partial class DetermineExistingCustomerActivity
{
    public override Task<BpmnWorkflowState> RunAsync(WorkflowActivityContext context, BpmnWorkflowState input)
    {
        throw new NotImplementedException();
    }
}
