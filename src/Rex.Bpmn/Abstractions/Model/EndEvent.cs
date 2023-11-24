using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tEndEvent", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("endEvent", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class EndEvent : ThrowEvent
    {
    }
}