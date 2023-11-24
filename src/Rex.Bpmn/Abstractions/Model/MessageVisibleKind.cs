using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType(Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
    public enum MessageVisibleKind
    {
        [XmlEnum("initiating")]
        Initiating,
        [XmlEnum("non_initiating")]
        NonInitiating
    }
}