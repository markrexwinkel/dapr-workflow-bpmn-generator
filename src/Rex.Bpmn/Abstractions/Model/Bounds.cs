using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.BpmnDC)]
[XmlRoot(Namespace = Namespaces.BpmnDC, IsNullable = false)]
public class Bounds
{
    [XmlAttribute("x")]
    public double X { get; set; }

    [XmlAttribute("y")]
    public double Y { get; set; }

    [XmlAttribute("width")]
    public double Width { get; set; }

    [XmlAttribute("height")]
    public double Height { get; set; }
}