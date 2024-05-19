using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tParticipantAssociation", Namespace = Namespaces.Bpmn)]
[XmlRoot("participantAssociation", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ParticipantAssociation : BaseElement
{
    [XmlElement("innerParticipantRef")]
    public XmlQualifiedName InnerParticipantRef { get; set; }

    [XmlElement("outerParticipantRef")]
    public XmlQualifiedName OuterParticipantRef { get; set; }
}