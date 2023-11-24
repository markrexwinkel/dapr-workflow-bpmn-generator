using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tPartnerEntity", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("partnerEntity", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class PartnerEntity : RootElement
    {
        private readonly Lazy<Collection<XmlQualifiedName>> _participantRefs = new Lazy<Collection<XmlQualifiedName>>();

        [XmlElement("participantRef")]
        public Collection<XmlQualifiedName> ParticipantRefs => _participantRefs.Value;

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}