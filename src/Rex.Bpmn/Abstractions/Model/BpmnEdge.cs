﻿using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("BPMNEdge", Namespace = Namespaces.BpmnDI)]
[XmlRoot("BPMNEdge", Namespace = Namespaces.BpmnDI, IsNullable = false)]
public class BpmnEdge : LabeledEdge
{
    [XmlElement("BPMNLabel")]
    public BpmnLabel Label { get; set; }

    [XmlAttribute("bpmnElement")]
    public XmlQualifiedName Element { get; set; }

    [XmlAttribute("sourceElement")]
    public XmlQualifiedName SourceElement { get; set; }

    [XmlAttribute("targetElement")]
    public XmlQualifiedName TargetElement { get; set; }

    [XmlAttribute("messageVisibleKind")]
    public MessageVisibleKind MessageVisibleKind { get; set; }

    [XmlIgnore]
    public bool MessageVisibleKindSpecified { get; set; }
}