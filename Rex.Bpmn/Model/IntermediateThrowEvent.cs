using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tIntermediateThrowEvent", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("intermediateThrowEvent", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class IntermediateThrowEvent : ThrowEvent
    {
    }
}