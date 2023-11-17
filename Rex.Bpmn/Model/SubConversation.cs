using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tSubConversation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("subConversation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class SubConversation : ConversationNode
    {
        private readonly Lazy<Collection<ConversationNode>> _conversationNodes = new Lazy<Collection<ConversationNode>>();

        [XmlElement(typeof(SubConversation))]
        [XmlElement(typeof(Conversation))]
        [XmlElement(typeof(CallConversation))]
        public Collection<ConversationNode> ConversationNodes => _conversationNodes.Value;
    }
}