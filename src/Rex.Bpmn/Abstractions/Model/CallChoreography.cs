using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tCallChoreography", Namespace = Namespaces.Bpmn)]
[XmlRoot("callChoreography", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class CallChoreography : ChoreographyActivity
{
    private readonly Lazy<Collection<ParticipantAssociation>> _participantAssociations = new();

    [XmlElement("participantAssociation")]
    public Collection<ParticipantAssociation> ParticipantAssociations => _participantAssociations.Value;

    [XmlAttribute("calledChoreographyRef")]
    public XmlQualifiedName CalledChoreographyRef { get; set; }
}