using Rex.Bpmn.Abstractions.Model;
using System.Collections.Generic;
using System.Linq;

namespace Rex.Bpmn.Abstractions
{
    public static class FlowElementExtensions
    {
        public static IEnumerable<(FlowElement element, SequenceFlow flow)> GetTargets(this FlowElement flowElement, Process process)
        {
            return process.FlowElements
                .OfType<SequenceFlow>()
                .Where(x => x.SourceRef == flowElement.Id)
                .SelectMany(x => process.FlowElements.Where(y => y.Id == x.TargetRef).Select(z => (z, x)));
        }

        public static IEnumerable<(FlowElement element, SequenceFlow flow)> GetSources(this FlowElement flowElement, Process process)
        {
            return process.FlowElements
                .OfType<SequenceFlow>()
                .Where(x => x.TargetRef == flowElement.Id)
                .SelectMany(x => process.FlowElements.Where(y => y.Id == x.SourceRef).Select(z => (z, x)));
        }
    }
}
