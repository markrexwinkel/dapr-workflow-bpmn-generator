using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tConditionalEventDefinition", Namespace = Namespaces.Bpmn)]
[XmlRoot("conditionalEventDefinition", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ConditionalEventDefinition : EventDefinition
{
    [XmlElement("condition")]
    public Expression Condition { get; set; }
}