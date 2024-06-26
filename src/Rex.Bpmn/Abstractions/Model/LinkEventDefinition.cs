﻿using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tLinkEventDefinition", Namespace = Namespaces.Bpmn)]
[XmlRoot("linkEventDefinition", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class LinkEventDefinition : EventDefinition
{
    private readonly Lazy<Collection<XmlQualifiedName>> _sources = new();

    [XmlElement("source")]
    public Collection<XmlQualifiedName> Sources => _sources.Value;

    [XmlElement("target")]
    public XmlQualifiedName Target { get; set; }

    [XmlAttribute]
    public string Name { get; set; }
}