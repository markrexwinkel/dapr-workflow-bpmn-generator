using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(BpmnPlane))]
[XmlType(Namespace="http://www.omg.org/spec/DD/20100524/DI")]
[XmlRoot(Namespace = "http://www.omg.org/spec/DD/20100524/DI", IsNullable = false)]
public abstract class Plane : Node
{
    private readonly Lazy<Collection<DiagramElement>> _elements = new();

    [XmlElement("BPMNEdge", Type = typeof(BpmnEdge), Namespace = Namespaces.BpmnDI)]
    [XmlElement("BPMNShape", Type = typeof(BpmnShape), Namespace = Namespaces.BpmnDI)]
    [XmlElement("BPMNLabel", Type = typeof(BpmnLabel), Namespace = Namespaces.BpmnDI)]
    [XmlElement("BPMNPlane", Type = typeof(BpmnPlane), Namespace = Namespaces.BpmnDI)]
    public Collection<DiagramElement> Elements => _elements.Value;
}