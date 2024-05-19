using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tTerminateEventDefinition", Namespace = Namespaces.Bpmn)]
[XmlRoot("terminateEventDefinition", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class TerminateEventDefinition : EventDefinition
{
}