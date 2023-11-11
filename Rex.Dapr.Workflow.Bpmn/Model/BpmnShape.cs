using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("BPMNShape", Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
    [XmlRoot("BPMNShape", Namespace = "http://www.omg.org/spec/BPMN/20100524/DI", IsNullable = false)]
    public class BpmnShape : LabeledShape
    {
        [XmlElement("BPMNLabel")]
        public BpmnLabel Label { get; set; }

        [XmlAttribute("bpmnElement")]
        public XmlQualifiedName Element { get; set; }

        [XmlAttribute("isHorizontal")]
        public bool IsHorizontal { get; set; }

        [XmlAttribute("isExpanded")]
        public bool IsExpanded { get; set; }

        [XmlAttribute("isMarkerVisible")]
        public bool IsMarkerVisible { get; set; }

        [XmlAttribute("isMessageVisible")]
        public bool IsMessageVisible { get; set; }

        [XmlAttribute("participantBandKind")]
        public ParticipantBandKind ParticipantBandKind { get; set; }

        [XmlIgnore]
        public bool ParticipantBandKindSpecified { get; set; }

        [XmlAttribute("choreographyActivityShape")]
        public XmlQualifiedName ChoreographyActivityShape { get; set; }
    }
}