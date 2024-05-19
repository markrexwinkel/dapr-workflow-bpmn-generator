using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tAssignment", Namespace = Namespaces.Bpmn)]
[XmlRoot("assignment", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Assignment : BaseElement
{
    [XmlElement("from")]
    public Expression From { get; set; }
    [XmlElement("to")]
    public Expression To { get; set; }
}
