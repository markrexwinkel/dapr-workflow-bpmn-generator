using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.Bpmn)]
[XmlRoot("extensionElements", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ExtensionElements
{
    private readonly Lazy<Collection<XmlElement>> _elements = new();

    [XmlAnyElement]
    public Collection<XmlElement> Elements  => _elements.Value;
}