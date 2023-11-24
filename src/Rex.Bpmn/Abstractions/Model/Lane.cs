using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tLane", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("lane", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Lane : BaseElement
    {
        private readonly Lazy<Collection<string>> _flowModeRefs = new Lazy<Collection<string>>();

        [XmlElement("partitionElement")]
        public BaseElement PartitionElement { get; set; }

        [XmlElement("flowModeRef", DataType = "IDREF")]
        public Collection<string> FlowModeRefs => _flowModeRefs.Value;

        [XmlElement("childLaneSet")]
        public LaneSet ChildLaneSet { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("partitionElementRef")]
        public XmlQualifiedName PartitionElementRef { get; set; }
    }
}