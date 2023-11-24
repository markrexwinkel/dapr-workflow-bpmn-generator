using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(ThrowEvent))]
    [XmlInclude(typeof(IntermediateThrowEvent))]
    [XmlInclude(typeof(ImplicitThrowEvent))]
    [XmlInclude(typeof(EndEvent))]
    [XmlInclude(typeof(CatchEvent))]
    [XmlInclude(typeof(StartEvent))]
    [XmlInclude(typeof(IntermediateCatchEvent))]
    [XmlInclude(typeof(BoundaryEvent))]
    [XmlType("tEvent", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("event", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public abstract class Event : FlowNode
    {
        private readonly Lazy<Collection<Property>> _properties = new Lazy<Collection<Property>>();

        [XmlElement("property")]
        public Collection<Property> Properties => _properties.Value;
    }
}