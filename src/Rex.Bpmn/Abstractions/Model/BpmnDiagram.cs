using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("BPMNDiagram", Namespace = Namespaces.BpmnDI)]
[XmlRoot("BPMNDiagram", Namespace = Namespaces.BpmnDI, IsNullable = false)]
public class BpmnDiagram : Diagram
{
    private readonly Lazy<Collection<BpmnLabelStyle>> _labelStyles = new();

    [XmlElement("BPMNPlane")]
    public BpmnPlane Plane { get; set;}

    [XmlElement("BPMNLabelStyle")]
    public Collection<BpmnLabelStyle> LabelStyles => _labelStyles.Value;
}