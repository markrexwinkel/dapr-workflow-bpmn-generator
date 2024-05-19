using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("BPMNLabel", Namespace = Namespaces.BpmnDI)]
[XmlRoot("BPMNLabel", Namespace = Namespaces.BpmnDI, IsNullable = false)]
public class BpmnLabel : Label
{
    [XmlAttribute("labelStyle")]
    public XmlQualifiedName LabelStyle { get; set; }
}