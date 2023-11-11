﻿using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tParticipant", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("participant", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Participant : BaseElement
    {
        private readonly Lazy<Collection<XmlQualifiedName>> _interfaceRefs = new Lazy<Collection<XmlQualifiedName>>();
        private readonly Lazy<Collection<XmlQualifiedName>> _endpointRefs = new Lazy<Collection<XmlQualifiedName>>();

        [XmlElement("interfaceRef")]
        public Collection<XmlQualifiedName> InterfaceRefs => _interfaceRefs.Value;

        [XmlElement("endPointRef")]
        public Collection<XmlQualifiedName> EndpointRefs => _endpointRefs.Value;

        [XmlElement("participantMultiplicity")]
        public ParticipantMultiplicity ParticipantMultiplicity { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("processDef")]
        public XmlQualifiedName ProcessDef { get; set; }


    }
}