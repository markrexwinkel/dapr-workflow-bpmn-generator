using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("BPMNPlane", Namespace = Namespaces.BpmnDI)]
[XmlRoot("BPMNPlane", Namespace = Namespaces.BpmnDI, IsNullable = false)]
public class BpmnPlane : Plane
{
    [XmlAttribute("bpmnElement")]
    public XmlQualifiedName Element { get; set; }
}