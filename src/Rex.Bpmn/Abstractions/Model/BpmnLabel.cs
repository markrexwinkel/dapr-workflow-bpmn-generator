using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("BPMNLabel", Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
    [XmlRoot("BPMNLabel", Namespace = "http://www.omg.org/spec/BPMN/20100524/DI", IsNullable = false)]
    public class BpmnLabel : Label
    {
        [XmlAttribute("labelStyle")]
        public XmlQualifiedName LabelStyle { get; set; }
    }
}