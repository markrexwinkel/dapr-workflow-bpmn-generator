using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.BpmnDI)]
public enum MessageVisibleKind
{
    [XmlEnum("initiating")]
    Initiating,
    [XmlEnum("non_initiating")]
    NonInitiating
}