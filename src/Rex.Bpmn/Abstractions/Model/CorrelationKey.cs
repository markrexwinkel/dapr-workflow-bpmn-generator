using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tCorrelationKey", Namespace = Namespaces.Bpmn)]
[XmlRoot("correlationKey", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class CorrelationKey : BaseElement
{
    private readonly Lazy<Collection<XmlQualifiedName>> _correlationPropertyRefs = new();

    [XmlElement("correlationPropertyRef")]
    public Collection<XmlQualifiedName> CorrelationPropertyRefs => _correlationPropertyRefs.Value;

    [XmlAttribute("name")]
    public string Name { get; set; }
}