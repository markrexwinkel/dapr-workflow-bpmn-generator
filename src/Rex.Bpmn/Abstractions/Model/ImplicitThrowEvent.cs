using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tImplicitThrowEvent", Namespace = Namespaces.Bpmn)]
[XmlRoot("implicitThrowEvent", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ImplicitThrowEvent : ThrowEvent
{
}