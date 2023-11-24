using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(BpmnPlane))]
    [XmlType(Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [XmlRoot(Namespace = "http://www.omg.org/spec/DD/20100524/DI", IsNullable = false)]
    public abstract class Plane : Node
    {
        private readonly Lazy<Collection<DiagramElement>> _elements = new Lazy<Collection<DiagramElement>>();

        [XmlElement("BPMNEdge", Type = typeof(BpmnEdge), Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
        [XmlElement("BPMNShape", Type = typeof(BpmnShape), Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
        [XmlElement("BPMNLabel", Type = typeof(BpmnLabel), Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
        [XmlElement("BPMNPlane", Type = typeof(BpmnPlane), Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
        public Collection<DiagramElement> Elements => _elements.Value;
    }
}