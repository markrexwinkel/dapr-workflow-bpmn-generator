using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tConversationAssociation", Namespace = Namespaces.Bpmn)]
[XmlRoot("conversationAssociation", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ConversationAssociation : BaseElement
{
    [XmlAttribute("innerConversationNodeRef")]
    public XmlQualifiedName InnerConversationNodeRef { get; set; }

    [XmlAttribute("outerConversationNodeRef")]
    public XmlQualifiedName OuterConversationNodeRef { get; set; }
}