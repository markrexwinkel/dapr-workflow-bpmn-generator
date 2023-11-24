using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tRelationship", Namespace = Namespaces.Bpmn)]
[XmlRoot("relationship", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Relationship : BaseElement
{
    private readonly Lazy<Collection<XmlQualifiedName>> _sources = new();
    private readonly Lazy<Collection<XmlQualifiedName>> _targets = new();

    [XmlElement("source")]
    public Collection<XmlQualifiedName> Sources => _sources.Value;

    [XmlElement("target")]
    public Collection<XmlQualifiedName> Targets => _targets.Value;

    [XmlAttribute("type")]
    public string Type { get; set; }

    [XmlAttribute("direction")]
    public RelationshipDirection Direction { get; set; }
}