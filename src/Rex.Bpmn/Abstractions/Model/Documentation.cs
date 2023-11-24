using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.Bpmn)]
[XmlRoot("documentation", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Documentation
{
    private readonly Lazy<Collection<XmlElement>> _extensionElements = new();

    [XmlAttribute("id")]
    public string Id { get; set; }

    [XmlAttribute("textFormat")]
    [DefaultValue("text/plain")]
    public string TextFormat { get; set; } = "text/plain";

    [XmlAnyElement]
    public Collection<XmlElement> ExtensionElements => _extensionElements.Value;
}