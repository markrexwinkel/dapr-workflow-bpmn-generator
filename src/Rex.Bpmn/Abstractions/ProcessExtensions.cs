using Rex.Bpmn.Abstractions.Model;
using System.Linq;

namespace Rex.Bpmn.Abstractions
{
    public static class ProcessExtensions
    {
        public static TFlowElement GetFlowElement<TFlowElement>(this IFlowElements process) where TFlowElement : FlowElement
        {
            return process.FlowElements.OfType<TFlowElement>().FirstOrDefault();
        }
    }
}
