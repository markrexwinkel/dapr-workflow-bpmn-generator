using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tParticipant", Namespace = Namespaces.Bpmn)]
[XmlRoot("participant", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Participant : BaseElement
{
    private readonly Lazy<Collection<XmlQualifiedName>> _interfaceRefs = new();
    private readonly Lazy<Collection<XmlQualifiedName>> _endpointRefs = new();

    [XmlElement("interfaceRef")]
    public Collection<XmlQualifiedName> InterfaceRefs => _interfaceRefs.Value;

    [XmlElement("endPointRef")]
    public Collection<XmlQualifiedName> EndpointRefs => _endpointRefs.Value;

    [XmlElement("participantMultiplicity")]
    public ParticipantMultiplicity ParticipantMultiplicity { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("processRef")]
    public XmlQualifiedName ProcessRef { get; set; }


}