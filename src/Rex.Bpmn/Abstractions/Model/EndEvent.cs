using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tEndEvent", Namespace = Namespaces.Bpmn)]
[XmlRoot("endEvent", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class EndEvent : ThrowEvent
{
}