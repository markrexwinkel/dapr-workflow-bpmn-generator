using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tCallConversation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("callConversation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class CallConversation : ConversationNode
    {
        private readonly Lazy<Collection<ParticipantAssociation>> _participantAssociations = new Lazy<Collection<ParticipantAssociation>>();

        [XmlElement("participantAssociation")]
        public Collection<ParticipantAssociation> ParticipantAssocations => _participantAssociations.Value;

        [XmlAttribute("calledCollaborationRef")]
        public XmlQualifiedName CalledCollaborationRef { get; set; }
    }
}