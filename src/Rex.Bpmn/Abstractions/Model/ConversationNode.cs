using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(SubConversation))]
[XmlInclude(typeof(Conversation))]
[XmlInclude(typeof(CallConversation))]
[XmlType("tConversationNode", Namespace = Namespaces.Bpmn)]
[XmlRoot("conversationNode", Namespace = Namespaces.Bpmn, IsNullable = false)]
public abstract class ConversationNode : BaseElement
{
    private readonly Lazy<Collection<XmlQualifiedName>> _participantRefs = new();
    private readonly Lazy<Collection<XmlQualifiedName>> _messageFlowRefs = new();
    private readonly Lazy<Collection<CorrelationKey>> _correlationKeys = new();

    [XmlElement("participantRef")]
    public Collection<XmlQualifiedName> ParticipantRefs => _participantRefs.Value;

    [XmlElement("messageFlowRef")]
    public Collection<XmlQualifiedName> MessageFlowRefs => _messageFlowRefs.Value;

    [XmlElement("correlationKey")]
    public Collection<CorrelationKey> CorrelationKeys => _correlationKeys.Value;

    [XmlAttribute("name")]
    public string Name { get; set; }
}