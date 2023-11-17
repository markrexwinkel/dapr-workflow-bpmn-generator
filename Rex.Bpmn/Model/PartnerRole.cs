using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tPartnerRole", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("partnerRole", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class PartnerRole : RootElement
    {
        private readonly Lazy<Collection<XmlQualifiedName>> _participantRefs = new Lazy<Collection<XmlQualifiedName>>();

        [XmlElement("participantRef")]
        public Collection<XmlQualifiedName> ParticipantRefs => _participantRefs.Value;

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}