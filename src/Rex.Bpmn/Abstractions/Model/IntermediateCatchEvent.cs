using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tIntermediateCatchEvent", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("intermediateCatchEvent", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class IntermediateCatchEvent : CatchEvent
    {
    }
}