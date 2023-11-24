using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tFormalExpression", Namespace = Namespaces.Bpmn)]
[XmlRoot("formalExpression", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class FormalExpression : Expression
{
    [XmlAttribute("language", DataType = "anyURI")]
    public string Language { get; set; }

    [XmlAttribute("evaluatesToTypeRef")]
    public XmlQualifiedName EvaluatesToTypeRef { get; set; }
}