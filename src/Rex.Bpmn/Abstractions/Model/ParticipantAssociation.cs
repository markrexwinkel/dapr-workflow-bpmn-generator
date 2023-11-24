using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tParticipantAssociation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("participantAssociation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ParticipantAssociation : BaseElement
    {
        [XmlElement("innerParticipantRef")]
        public XmlQualifiedName InnerParticipantRef { get; set; }

        [XmlElement("outerParticipantRef")]
        public XmlQualifiedName OuterParticipantRef { get; set; }
    }
}