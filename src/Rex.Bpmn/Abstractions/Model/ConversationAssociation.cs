using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tConversationAssociation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("conversationAssociation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ConversationAssociation : BaseElement
    {
        [XmlAttribute("innerConversationNodeRef")]
        public XmlQualifiedName InnerConversationNodeRef { get; set; }

        [XmlAttribute("outerConversationNodeRef")]
        public XmlQualifiedName OuterConversationNodeRef { get; set; }
    }
}