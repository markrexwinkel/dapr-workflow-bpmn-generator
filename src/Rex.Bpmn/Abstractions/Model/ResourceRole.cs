using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(Performer))]
[XmlType("tResourceRole", Namespace = Namespaces.Bpmn)]
[XmlRoot("resourceRole", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ResourceRole : BaseElement
{
    private readonly Lazy<Collection<object>> _items = new();

    [XmlElement("resourceAssignmentExpression", typeof(ResourceAssignmentExpression))]
    [XmlElement("resourceParameterBinding", typeof(ResourceParameterBinding))]
    [XmlElement("resourceRef", typeof(XmlQualifiedName))]
    public Collection<object> Items => _items.Value;

    [XmlAttribute("name")]
    public string Name { get; set; }
}