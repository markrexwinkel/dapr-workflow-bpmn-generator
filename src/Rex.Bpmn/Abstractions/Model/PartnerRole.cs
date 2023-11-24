using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tPartnerRole", Namespace = Namespaces.Bpmn)]
[XmlRoot("partnerRole", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class PartnerRole : RootElement
{
    private readonly Lazy<Collection<XmlQualifiedName>> _participantRefs = new();

    [XmlElement("participantRef")]
    public Collection<XmlQualifiedName> ParticipantRefs => _participantRefs.Value;

    [XmlAttribute("name")]
    public string Name { get; set; }
}