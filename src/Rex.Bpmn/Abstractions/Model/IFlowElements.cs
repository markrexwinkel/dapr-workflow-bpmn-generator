using System.Collections.ObjectModel;

namespace Rex.Bpmn.Abstractions.Model
{
    public interface IFlowElements : IIdentifiable, IExtensible
    {
        Collection<FlowElement> FlowElements { get; }
    }
}
