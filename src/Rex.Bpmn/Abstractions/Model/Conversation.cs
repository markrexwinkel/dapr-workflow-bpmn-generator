using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tConversation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("conversation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Conversation : ConversationNode
    {
    }
}