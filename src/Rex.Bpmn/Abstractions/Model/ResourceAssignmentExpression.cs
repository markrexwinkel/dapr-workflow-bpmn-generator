using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tResourceAssignmentExpression", Namespace = Namespaces.Bpmn)]
[XmlRoot("resourceAssignmentExpression", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ResourceAssignmentExpression : BaseElement
{
    [XmlElement("expression")]
    public Expression Expression { get; set; }
}