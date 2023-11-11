using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("BPMNEdge", Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
    [XmlRoot("BPMNEdge", Namespace = "http://www.omg.org/spec/BPMN/20100524/DI", IsNullable = false)]
    public class BpmnEdge : LabeledEdge
    {
        [XmlElement("BPMNLabel")]
        public BpmnLabel Label { get; set; }

        [XmlAttribute("bpmnElement")]
        public XmlQualifiedName Element { get; set; }

        [XmlAttribute("sourceElement")]
        public XmlQualifiedName SourceElement { get; set; }

        [XmlAttribute("targetElement")]
        public XmlQualifiedName TargetElement { get; set; }

        [XmlAttribute("messageVisibleKind")]
        public MessageVisibleKind MessageVisibleKind { get; set; }

        [XmlIgnore]
        public bool MessageVisibleKindSpecified { get; set; }
    }
}