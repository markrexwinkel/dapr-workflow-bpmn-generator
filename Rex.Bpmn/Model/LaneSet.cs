using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tLaneSet", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("laneSet", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class LaneSet : BaseElement
    {
        private readonly Lazy<Collection<Lane>> _lanes = new Lazy<Collection<Lane>>();

        [XmlElement("lane")]
        public Collection<Lane> Lanes => _lanes.Value;

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}