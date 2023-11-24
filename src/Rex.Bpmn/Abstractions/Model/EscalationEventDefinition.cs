using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tEscalationEventDefinition", Namespace = Namespaces.Bpmn)]
[XmlRoot("escalationEventDefinition", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class EscalationEventDefinition : EventDefinition
{
    [XmlAttribute("escalationRef")]
    public XmlQualifiedName EscalationRef { get; set; }
}