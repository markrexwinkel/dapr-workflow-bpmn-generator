using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType(Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
    public enum ParticipantBandKind
    {
        [XmlEnum("top_initiating")]
        TopInitiating,
        [XmlEnum("middle_initiating")]
        MiddleInitiating,
        [XmlEnum("bottom_initiating")]
        BottomInitiating,
        [XmlEnum("top_non_initiating")]
        TopNonInitiating,
        [XmlEnum("middle_non_initiating")]
        MiddleNonInitiating,
        [XmlEnum("bottom_non_initiating")]
        BottomNonInitiating
    }
}