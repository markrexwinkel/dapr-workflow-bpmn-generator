using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(Plane))]
[XmlInclude(typeof(BpmnPlane))]
[XmlInclude(typeof(Label))]
[XmlInclude(typeof(BpmnLabel))]
[XmlInclude(typeof(Shape))]
[XmlInclude(typeof(LabeledShape))]
[XmlInclude(typeof(BpmnShape))]
[XmlType(Namespace = "http://www.omg.org/spec/DD/20100524/DI")]
[XmlRoot(Namespace = "http://www.omg.org/spec/DD/20100524/DI", IsNullable = false)]
public abstract class Node : DiagramElement
{
}