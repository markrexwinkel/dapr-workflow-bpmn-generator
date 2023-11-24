using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tChoreographyLoopType", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public enum ChoreographyLoopType
    {
        None,
        Standard,
        MultiInstanceSequential,
        MultiInstanceParallel
    }
}