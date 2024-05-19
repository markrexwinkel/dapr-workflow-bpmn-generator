using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tSubConversation", Namespace = Namespaces.Bpmn)]
[XmlRoot("subConversation", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class SubConversation : ConversationNode
{
    private readonly Lazy<Collection<ConversationNode>> _conversationNodes = new();

    [XmlElement(typeof(SubConversation))]
    [XmlElement(typeof(Conversation))]
    [XmlElement(typeof(CallConversation))]
    public Collection<ConversationNode> ConversationNodes => _conversationNodes.Value;
}