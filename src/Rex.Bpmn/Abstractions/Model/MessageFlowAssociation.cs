using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tMessageFlowAssociation", Namespace = Namespaces.Bpmn)]
[XmlRoot("messageFlowAssociation", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class MessageFlowAssociation : BaseElement
{
    [XmlAttribute("innerMessageFlowRef")]
    public XmlQualifiedName InnerMessageFlowRef { get; set; }

    [XmlAttribute("outerMessageFlowRef")]
    public XmlQualifiedName OuterMessageFlowRef { get; set; }
}