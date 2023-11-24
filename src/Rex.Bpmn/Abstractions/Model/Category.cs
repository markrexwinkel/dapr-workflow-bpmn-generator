using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tCategory", Namespace = Namespaces.Bpmn)]
[XmlRoot("category", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Category : RootElement
{
    private readonly Lazy<Collection<CategoryValue>> _values = new();

    [XmlElement("categoryValue")]
    public Collection<CategoryValue> Values => _values.Value;

    [XmlAttribute("name")]
    public string Name { get; set; }
}