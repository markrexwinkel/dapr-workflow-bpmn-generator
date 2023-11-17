using TestApp.Workflows.Activities;
using Dapr.Workflow;
using Rex.Dapr.Workflow.Bpmn;
using System;
using System.Collections.Generic;

namespace TestApp.Workflows
{

    public class LoanApplicationWorkflow2 : Workflow<BpmnWorkflowState, BpmnWorkflowState>
    {
        private delegate Task<CallHandlerAsync[]> CallHandlerAsync(WorkflowContext context, BpmnWorkflowState state);

        public override async Task<BpmnWorkflowState> RunAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            List<Task<CallHandlerAsync[]>> tasks = [CallDetermineExistingCustomerAsync(context, state)];
            while(tasks.Count > 0)
            {
                var readyTask = await Task.WhenAny(tasks);
                tasks.AddRange((await readyTask).Select(x => x(context, state)));
            }
            return state;
        }


        private Task<CallHandlerAsync[]> Callgwy_ExistingCustomerAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            if (state["CustomerInfo"] is null)
            {
                return Task.FromResult<CallHandlerAsync[]>([CallRegisterProspectAsync]);
            }
            else
            {
                return Task.FromResult<CallHandlerAsync[]>([CallGateway_02c1v2mAsync]);
            }
        }

        private Task<CallHandlerAsync[]> CallGateway_02c1v2mAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            return Task.FromResult<CallHandlerAsync[]>([CallDetermineRiskProfileAsync]);
        }

        private Task<CallHandlerAsync[]> Callgwy_LoanApprovedAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            if ((bool)state["LoanApproved"])
            {
                return Task.FromResult<CallHandlerAsync[]>([CallSendProposalAsync]);
            }
            else
            {
                return Task.FromResult<CallHandlerAsync[]>([CallSendRejectionLetterAsync]);
            }
        }

        private Task<CallHandlerAsync[]> CallGateway_10ptwt1Async(WorkflowContext context, BpmnWorkflowState state)
        {
            return Task.FromResult<CallHandlerAsync[]>([CallEvent_0m2nbrjAsync]);
        }

        private Task<CallHandlerAsync[]> CallEvent_0m2nbrjAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            return Task.FromResult(Array.Empty<CallHandlerAsync>());
        }

        private async Task<CallHandlerAsync[]> CallAssessApplicationAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            state.Merge(await context.WaitForExternalEventAsync<BpmnWorkflowState>("AssessApplicationCompleted"));
            return [Callgwy_LoanApprovedAsync];
        }

        private async Task<CallHandlerAsync[]> CallDetermineRiskProfileAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            state.Merge(await context.CallActivityAsync<BpmnWorkflowState>(nameof(DetermineRiskProfileActivity), state));
            return [CallGateway_13kw37tAsync];
        }

        private async Task<CallHandlerAsync[]> CallRegisterProspectAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            state.Merge(await context.CallActivityAsync<BpmnWorkflowState>(nameof(RegisterProspectActivity), state));
            return [CallGateway_02c1v2mAsync];
        }

        private async Task<CallHandlerAsync[]> CallDetermineExistingCustomerAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            state.Merge(await context.CallActivityAsync<BpmnWorkflowState>(nameof(DetermineExistingCustomerActivity), state));
            return [Callgwy_ExistingCustomerAsync];
        }

        private async Task<CallHandlerAsync[]> CallRegisterContractAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            state.Merge(await context.CallActivityAsync<BpmnWorkflowState>(nameof(RegisterContractActivity), state));
            return [CallSendContractAsync];
        }

        private async Task<CallHandlerAsync[]> CallSendRejectionLetterAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            state.Merge(await context.CallActivityAsync<BpmnWorkflowState>(nameof(SendRejectionLetterActivity), state));
            return [CallGateway_10ptwt1Async];
        }

        private async Task<CallHandlerAsync[]> CallSendContractAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            state.Merge(await context.CallActivityAsync<BpmnWorkflowState>(nameof(SendContractActivity), state));
            return [CallGateway_10ptwt1Async];
        }

        private async Task<CallHandlerAsync[]> CallSendProposalAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            state.Merge(await context.CallActivityAsync<BpmnWorkflowState>(nameof(SendProposalActivity), state));
            return [CallReceiveCustomerDecisionAsync];
        }

        private async Task<CallHandlerAsync[]> CallReceiveCustomerDecisionAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            try
            {
                state.Merge(await context.WaitForExternalEventAsync<BpmnWorkflowState>("{{receiveTask.Id}}Completed", TimeSpan.FromTicks(12096000000000)));
                return [Callgwy_ProposalAcceptedAsync];
            }
            catch (TaskCanceledException)
            {
                return [CallContactCustomerAsync];
            }
        }

        private Task<CallHandlerAsync[]> Callgwy_ProposalAcceptedAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            if ((bool)state["ProposalAccepted"])
            {
                return Task.FromResult<CallHandlerAsync[]>([CallRegisterContractAsync]);
            }
            else
            {
                return Task.FromResult<CallHandlerAsync[]>([CallGateway_10ptwt1Async]);
            }
        }

        private async Task<CallHandlerAsync[]> CallContactCustomerAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            state.Merge(await context.WaitForExternalEventAsync<BpmnWorkflowState>("ContactCustomerCompleted"));
            return [Callgwy_ProposalAcceptedAsync];
        }


        private Task<CallHandlerAsync[]> CallGateway_13kw37tAsync(WorkflowContext context, BpmnWorkflowState state)
        {
            if ((int)state["RiskClass"] > 3)
            {
                return Task.FromResult<CallHandlerAsync[]>([CallAssessApplicationAsync]);
            }
            else
            {
                return Task.FromResult<CallHandlerAsync[]>([Callgwy_LoanApprovedAsync]);
            }
        }
    }
}

