using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("BPMNPlane", Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
    [XmlRoot("BPMNPlane", Namespace = "http://www.omg.org/spec/BPMN/20100524/DI", IsNullable = false)]
    public class BpmnPlane : Plane
    {
        [XmlAttribute("bpmnElement")]
        public XmlQualifiedName Element { get; set; }
    }
}