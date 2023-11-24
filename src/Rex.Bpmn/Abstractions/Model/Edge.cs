using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(LabeledEdge))]
    [XmlInclude(typeof(BpmnEdge))]
    [XmlType(Namespace = "http://www.omg.org/spec/DD/20100524/DI")]
    [XmlRoot(Namespace = "http://www.omg.org/spec/DD/20100524/DI", IsNullable = false)]
    public abstract class Edge : DiagramElement
    {
        private readonly Lazy<Collection<Point>> _wayPoints = new Lazy<Collection<Point>>();

        [XmlElement("waypoint")]
        public Collection<Point> WayPoints => _wayPoints.Value;
    }
}