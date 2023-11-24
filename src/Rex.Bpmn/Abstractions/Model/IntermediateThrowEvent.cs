using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tIntermediateThrowEvent", Namespace = Namespaces.Bpmn)]
[XmlRoot("intermediateThrowEvent", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class IntermediateThrowEvent : ThrowEvent
{
}