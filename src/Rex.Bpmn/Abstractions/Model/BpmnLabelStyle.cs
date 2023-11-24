using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("BPMNLabelStyle", Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
    [XmlRoot("BPMNLabelStyle", Namespace = "http://www.omg.org/spec/BPMN/20100524/DI", IsNullable = false)]
    public class BpmnLabelStyle : Style
    {
        [XmlElement(Namespace = "http://www.omg.org/spec/DD/20100524/DC")]
        public Font Font { get; set; }
    }
}