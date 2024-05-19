using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(FormalExpression))]
[XmlType("tExpression", Namespace = Namespaces.Bpmn)]
[XmlRoot("expression", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Expression : BaseElementWithMixedContent
{
}