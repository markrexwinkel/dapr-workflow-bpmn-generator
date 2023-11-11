using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tParticipantMultiplicity", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("participantMultiplicity", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ParticipantMultiplicity : BaseElement
    {
        [XmlAttribute("minimum")]
        [DefaultValue(0)]
        public int Minimum { get; set; } = 0;

        [XmlAttribute("maximum")]
        [DefaultValue(1)]
        public int Maximum { get; set; } = 1;
    }
}