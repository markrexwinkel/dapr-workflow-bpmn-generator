using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tAuditing", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("auditing", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Auditing : BaseElement
    {
    }
}
