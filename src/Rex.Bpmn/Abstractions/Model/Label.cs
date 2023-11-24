using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(BpmnLabel))]
[XmlType(Namespace = "http://www.omg.org/spec/DD/20100524/DI")]
[XmlRoot(Namespace = "http://www.omg.org/spec/DD/20100524/DI", IsNullable = false)]
public abstract class Label : Node
{
    [XmlElement(Namespace = Namespaces.BpmnDC)]
    public Bounds Bounds { get; set; }
}