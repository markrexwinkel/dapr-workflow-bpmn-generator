using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tPartnerEntity", Namespace = Namespaces.Bpmn)]
[XmlRoot("partnerEntity", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class PartnerEntity : RootElement
{
    private readonly Lazy<Collection<XmlQualifiedName>> _participantRefs = new();

    [XmlElement("participantRef")]
    public Collection<XmlQualifiedName> ParticipantRefs => _participantRefs.Value;

    [XmlAttribute("name")]
    public string Name { get; set; }
}