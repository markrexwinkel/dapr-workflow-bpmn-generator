using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tMessageEventDefinition", Namespace = Namespaces.Bpmn)]
[XmlRoot("messageEventDefinition", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class MessageEventDefinition : EventDefinition
{
    [XmlElement("operationRef")]
    public XmlQualifiedName OperationRef { get; set; }

    [XmlAttribute("messageRef")]
    public XmlQualifiedName MessageRef { get; set; }
}