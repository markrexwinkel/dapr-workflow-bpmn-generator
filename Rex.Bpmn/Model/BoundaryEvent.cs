using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tBoundaryEvent", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("boundaryEvent", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class BoundaryEvent : CatchEvent
    {
        [XmlAttribute("cancelActivity")]
        [DefaultValue(true)]
        public bool CancelActivity { get; set; } = true;

        [XmlAttribute("attachedToRef")]
        public XmlQualifiedName AttachedToRef { get; set; }
    }
}