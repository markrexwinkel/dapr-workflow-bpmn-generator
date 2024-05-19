using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tCancelEventDefinition", Namespace = Namespaces.Bpmn)]
[XmlRoot("cancelEventDefinition", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class CancelEventDefinition : EventDefinition
{
}