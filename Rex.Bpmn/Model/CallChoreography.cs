using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tCallChoreography", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("callChoreography", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class CallChoreography : ChoreographyActivity
    {
        private readonly Lazy<Collection<ParticipantAssociation>> _participantAssociations = new Lazy<Collection<ParticipantAssociation>>();

        [XmlElement("participantAssociation")]
        public Collection<ParticipantAssociation> ParticipantAssociations => _participantAssociations.Value;

        [XmlAttribute("calledChoreographyRef")]
        public XmlQualifiedName CalledChoreographyRef { get; set; }
    }
}