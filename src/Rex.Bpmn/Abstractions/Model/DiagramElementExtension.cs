﻿using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(AnonymousType = true, Namespace = "http://www.omg.org/spec/DD/20100524/DI")]
public class DiagramElementExtension
{
    private readonly Lazy<Collection<XmlElement>> _elements = new();

    [XmlAnyElement]
    public Collection<XmlElement> Elements => _elements.Value;
}