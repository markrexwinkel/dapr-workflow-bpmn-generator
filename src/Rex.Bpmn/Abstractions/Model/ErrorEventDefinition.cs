using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tErrorEventDefinition", Namespace = Namespaces.Bpmn)]
[XmlRoot("errorEventDefinition", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ErrorEventDefinition : EventDefinition
{
    [XmlAttribute("errorRef")]
    public XmlQualifiedName ErrorRef { get; set; }
}