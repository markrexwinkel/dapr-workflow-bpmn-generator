using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tCallConversation", Namespace = Namespaces.Bpmn)]
[XmlRoot("callConversation", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class CallConversation : ConversationNode
{
    private readonly Lazy<Collection<ParticipantAssociation>> _participantAssociations = new();

    [XmlElement("participantAssociation")]
    public Collection<ParticipantAssociation> ParticipantAssocations => _participantAssociations.Value;

    [XmlAttribute("calledCollaborationRef")]
    public XmlQualifiedName CalledCollaborationRef { get; set; }
}