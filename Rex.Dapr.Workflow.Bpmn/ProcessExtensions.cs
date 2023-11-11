using Rex.Bpmn.Model;
using System.Linq;

namespace Rex.Dapr.Workflow.Bpmn
{
    public static class ProcessExtensions
    {
        public static TFlowElement GetFlowElement<TFlowElement>(this Process process) where TFlowElement : FlowElement
        {
            return process.FlowElements.OfType<TFlowElement>().FirstOrDefault();
        }
    }
}
