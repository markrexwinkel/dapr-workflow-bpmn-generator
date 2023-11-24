using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(GlobalConversation))]
    [XmlInclude(typeof(Choreography))]
    [XmlInclude(typeof(GlobalChoreographyTask))]
    [XmlType("tCollaboration", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("collaboration", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Collaboration : RootElement
    {
        private readonly Lazy<Collection<Participant>> _participants = new Lazy<Collection<Participant>>();
        private readonly Lazy<Collection<MessageFlow>> _messageFlows = new Lazy<Collection<MessageFlow>>();
        private readonly Lazy<Collection<Artifact>> _artifacts = new Lazy<Collection<Artifact>>();
        private readonly Lazy<Collection<ConversationNode>> _conversationNodes = new Lazy<Collection<ConversationNode>>();
        private readonly Lazy<Collection<ConversationAssociation>> _conversationAssociations = new Lazy<Collection<ConversationAssociation>>();
        private readonly Lazy<Collection<ParticipantAssociation>> _participantAssociations = new Lazy<Collection<ParticipantAssociation>>();
        private readonly Lazy<Collection<MessageFlowAssociation>> _messageFlowAssociations = new Lazy<Collection<MessageFlowAssociation>>();
        private readonly Lazy<Collection<CorrelationKey>> _correlationKeys = new Lazy<Collection<CorrelationKey>>();
        private readonly Lazy<Collection<XmlQualifiedName>> _choreographyRefs = new Lazy<Collection<XmlQualifiedName>>();
        private readonly Lazy<Collection<ConversationLink>> _conversationLinks = new Lazy<Collection<ConversationLink>>();

        [XmlElement("participant")]
        public Collection<Participant> Participants => _participants.Value;

        [XmlElement("messageFlow")]
        public Collection<MessageFlow> MessageFlows => _messageFlows.Value;

        [XmlElement("textAnnotation", typeof(TextAnnotation))]
        [XmlElement("group", typeof(Group))]
        [XmlElement("association", typeof(Association))]
        public Collection<Artifact> Artifacts => _artifacts.Value;

        [XmlElement("subConversation", typeof(SubConversation))]
        [XmlElement("conversation", typeof(Conversation))]
        [XmlElement("callConversation", typeof(CallConversation))]
        public Collection<ConversationNode> ConversationNodes => _conversationNodes.Value;

        [XmlElement("conversationAssociation")]
        public Collection<ConversationAssociation> ConversationAssociations => _conversationAssociations.Value;

        [XmlElement("participantAssociation")]
        public Collection<ParticipantAssociation> ParticipantAssociations => _participantAssociations.Value;

        [XmlElement("messageFlowAssociation")]
        public Collection<MessageFlowAssociation> MessageFlowAssociations => _messageFlowAssociations.Value;

        [XmlElement("correlationKey")]
        public Collection<CorrelationKey> CorrelationKeys => _correlationKeys.Value;

        [XmlElement("choreographyRef")]
        public Collection<XmlQualifiedName> ChoreographyRefs => _choreographyRefs.Value;

        [XmlElement("conversationLink")]
        public Collection<ConversationLink> ConversationLinks => _conversationLinks.Value;

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("isClosed")]
        public bool IsClosed { get; set; }
    }
}