using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tIntermediateCatchEvent", Namespace = Namespaces.Bpmn)]
[XmlRoot("intermediateCatchEvent", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class IntermediateCatchEvent : CatchEvent
{
}