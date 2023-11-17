//using TestApp.Workflows.Activities;
using Dapr.Workflow;
using Rex.Dapr.Workflow.Bpmn;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using TestApp.Models;

namespace TestApp.Workflows
{
    public class LoanApplicationWorkflowState
    {
        public LoanApplication LoanApplication { get; set; }
        public ApplicationResult ApplicationResult { get; set; }
        public CustomerInfo CustomerInfo { get; set; }
        public RiskProfile RiskProfile { get; set; }
        public bool ProposalAccepted { get; set; }
    }

    public partial class DetermineExistingCustomerActivity : WorkflowActivity<LoanApplicationWorkflowState, CustomerInfo>
    {
        public override Task<CustomerInfo> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
        {
            throw new NotImplementedException();
        }
    }

    public partial class RegisterProspectActivity : WorkflowActivity<LoanApplicationWorkflowState, CustomerInfo>
    {
        public override Task<CustomerInfo> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
        {
            throw new NotImplementedException();
        }
    }

    public partial class DetermineRiskProfileActivity : WorkflowActivity<LoanApplicationWorkflowState, RiskProfile>
    {
        public override Task<RiskProfile> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
        {
            throw new NotImplementedException();
        }
    }

    public partial class SendProposalActivity : WorkflowActivity<LoanApplicationWorkflowState, object>
    {
        public override Task<object> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
        {
            throw new NotImplementedException();
        }
    }

    public partial class SendRejectionLetterActivity : WorkflowActivity<LoanApplicationWorkflowState, object>
    {
        public override Task<object> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
        {
            throw new NotImplementedException();
        }
    }

    public partial class RegisterContractActivity : WorkflowActivity<LoanApplicationWorkflowState, object>
    {
        public override Task<object> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
        {
            throw new NotImplementedException();
        }
    }

    public partial class SendContractActivity : WorkflowActivity<LoanApplicationWorkflowState, object>
    {
        public override Task<object> RunAsync(WorkflowActivityContext context, LoanApplicationWorkflowState input)
        {
            throw new NotImplementedException();
        }
    }

    public class LoanApplicationWorkflow2 : Workflow<TestApp.Models.LoanApplication, TestApp.Models.ApplicationResult>
    {
        

        private delegate Task<CallHandlerAsync[]> CallHandlerAsync(WorkflowContext context, LoanApplicationWorkflowState state);

        public override async Task<TestApp.Models.ApplicationResult> RunAsync(WorkflowContext context, TestApp.Models.LoanApplication loanApplication)
        {
            var state = new LoanApplicationWorkflowState
            {
                LoanApplication = loanApplication,
                ApplicationResult = new(),
            };
            var tasks = new List<Task<CallHandlerAsync[]>>() { CallLoanApplicationReceivedAsync(context, state) };
            while (tasks.Count > 0)
            {
                var readyTask = await Task.WhenAny(tasks);
                tasks.AddRange((await readyTask).Select(x => x(context, state)));
            }
            return state.ApplicationResult;
        }

        private Task<CallHandlerAsync[]> CallLoanApplicationReceivedAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            return Task.FromResult(new CallHandlerAsync[] { CallDetermineExistingCustomerAsync });
        }

        private Task<CallHandlerAsync[]> Callgwy_ExistingCustomerAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            if (state.CustomerInfo is null)
            {
                return Task.FromResult<CallHandlerAsync[]>(new CallHandlerAsync[] { CallRegisterProspectAsync });
            }
            else
            {
                return Task.FromResult<CallHandlerAsync[]>(new CallHandlerAsync[] { CallGateway_02c1v2mAsync });
            }
        }

        private Task<CallHandlerAsync[]> CallGateway_02c1v2mAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            return Task.FromResult<CallHandlerAsync[]>(new CallHandlerAsync[] { CallDetermineRiskProfileAsync });
        }

        private Task<CallHandlerAsync[]> Callgwy_LoanApprovedAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            if (state.ApplicationResult.Approved)
            {
                return Task.FromResult<CallHandlerAsync[]>(new CallHandlerAsync[] { CallSendProposalAsync });
            }
            else
            {
                return Task.FromResult<CallHandlerAsync[]>(new CallHandlerAsync[] { CallSendRejectionLetterAsync });
            }
        }

        private Task<CallHandlerAsync[]> CallGateway_10ptwt1Async(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            return Task.FromResult<CallHandlerAsync[]>(new CallHandlerAsync[] { CallEvent_0m2nbrjAsync });
        }

        private Task<CallHandlerAsync[]> CallEvent_0m2nbrjAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            return Task.FromResult(Array.Empty<CallHandlerAsync>());
        }

        private async Task<CallHandlerAsync[]> CallAssessApplicationAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            state.ApplicationResult.Approved = await context.WaitForExternalEventAsync<bool>("AssessApplicationCompleted");
            return new CallHandlerAsync[] { Callgwy_LoanApprovedAsync };
        }

        private async Task<CallHandlerAsync[]> CallDetermineRiskProfileAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            state.RiskProfile = await context.CallActivityAsync<RiskProfile>(nameof(DetermineRiskProfileActivity), state);
            return new CallHandlerAsync[] { CallGateway_13kw37tAsync };
        }

        private async Task<CallHandlerAsync[]> CallRegisterProspectAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            state.CustomerInfo = await context.CallActivityAsync<CustomerInfo>(nameof(RegisterProspectActivity), state.LoanApplication.ApplicantName);
            return new CallHandlerAsync[] { CallGateway_02c1v2mAsync };
        }

        private async Task<CallHandlerAsync[]> CallDetermineExistingCustomerAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            state.CustomerInfo = await context.CallActivityAsync<CustomerInfo>(nameof(DetermineExistingCustomerActivity), state);
            return new CallHandlerAsync[] { Callgwy_ExistingCustomerAsync };
        }

        private async Task<CallHandlerAsync[]> CallRegisterContractAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            await context.CallActivityAsync<object>(nameof(RegisterContractActivity), state);
            return new CallHandlerAsync[] { CallSendContractAsync };
        }

        private async Task<CallHandlerAsync[]> CallSendRejectionLetterAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            await context.CallActivityAsync<object>(nameof(SendRejectionLetterActivity), state);
            return new CallHandlerAsync[] { CallGateway_10ptwt1Async };
        }

        private async Task<CallHandlerAsync[]> CallSendContractAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            await context.CallActivityAsync<object>(nameof(SendContractActivity), state);
            return new CallHandlerAsync[] { CallGateway_10ptwt1Async };
        }

        private async Task<CallHandlerAsync[]> CallSendProposalAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            await context.CallActivityAsync<object>(nameof(SendProposalActivity), state);
            return new CallHandlerAsync[] { CallReceiveCustomerDecisionAsync };
        }

        private async Task<CallHandlerAsync[]> CallReceiveCustomerDecisionAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            try
            {
                state.ProposalAccepted = await context.WaitForExternalEventAsync<bool>("ReceiveCustomerDecisionCompleted", TimeSpan.FromTicks(12096000000000));
                return new CallHandlerAsync[] { Callgwy_ProposalAcceptedAsync };
            }
            catch (TaskCanceledException)
            {
                return new CallHandlerAsync[] { CallContactCustomerAsync };
            }

        }

        private Task<CallHandlerAsync[]> Callgwy_ProposalAcceptedAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            if (state.ProposalAccepted)
            {
                return Task.FromResult<CallHandlerAsync[]>(new CallHandlerAsync[] { CallRegisterContractAsync });
            }
            else
            {
                return Task.FromResult<CallHandlerAsync[]>(new CallHandlerAsync[] { CallGateway_10ptwt1Async });
            }
        }

        private async Task<CallHandlerAsync[]> CallContactCustomerAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            state.ProposalAccepted = await context.WaitForExternalEventAsync<bool>("ContactCustomerCompleted");
            return new CallHandlerAsync[] { Callgwy_ProposalAcceptedAsync };
        }

        private Task<CallHandlerAsync[]> CallGateway_13kw37tAsync(WorkflowContext context, LoanApplicationWorkflowState state)
        {
            if (state.RiskProfile.RiskClass > 3)
            {
                return Task.FromResult<CallHandlerAsync[]>(new CallHandlerAsync[] { CallAssessApplicationAsync });
            }
            else
            {
                return Task.FromResult<CallHandlerAsync[]>(new CallHandlerAsync[] { Callgwy_LoanApprovedAsync });
            }
        }
    }
}
