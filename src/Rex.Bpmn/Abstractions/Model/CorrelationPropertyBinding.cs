using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tCorrelationPropertyBinding", Namespace = Namespaces.Bpmn)]
[XmlRoot("correlationPropertyBinding", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class CorrelationPropertyBinding : BaseElement
{
    [XmlElement("dataPath")]
    public FormalExpression DataPath { get; set; }

    [XmlAttribute("correlationPropertyRef")]
    public XmlQualifiedName CorrelationPropertyRef { get; set; }
}