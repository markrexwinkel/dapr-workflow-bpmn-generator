using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("BPMNLabelStyle", Namespace = Namespaces.BpmnDI)]
[XmlRoot("BPMNLabelStyle", Namespace = Namespaces.BpmnDI, IsNullable = false)]
public class BpmnLabelStyle : Style
{
    [XmlElement(Namespace = Namespaces.BpmnDC)]
    public Font Font { get; set; }
}