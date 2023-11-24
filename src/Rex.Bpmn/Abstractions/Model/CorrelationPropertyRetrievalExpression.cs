using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tCorrelationPropertyRetrievalExpression", Namespace = Namespaces.Bpmn)]
[XmlRoot("correlationPropertyRetrievalExpression", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class CorrelationPropertyRetrievalExpression : BaseElement
{
    [XmlElement("messagePath")]
    public FormalExpression MessagePath { get; set; }

    [XmlAttribute("messageRef")]
    public XmlQualifiedName MessageRef { get; set; }
}