using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tLaneSet", Namespace = Namespaces.Bpmn)]
[XmlRoot("laneSet", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class LaneSet : BaseElement
{
    private readonly Lazy<Collection<Lane>> _lanes = new();

    [XmlElement("lane")]
    public Collection<Lane> Lanes => _lanes.Value;

    [XmlAttribute("name")]
    public string Name { get; set; }
}