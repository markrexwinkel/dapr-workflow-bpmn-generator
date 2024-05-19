using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tOperation", Namespace = Namespaces.Bpmn)]
[XmlRoot("operation", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Operation : BaseElement
{
    private readonly Lazy<Collection<XmlQualifiedName>> _errorRefs = new();

    [XmlElement("inMessageRef")]
    public XmlQualifiedName InMessageRef { get; set; }

    [XmlElement("outMessageRef")]
    public XmlQualifiedName OutMessageRef { get; set; }

    [XmlElement("errorRef")]
    public Collection<XmlQualifiedName> ErrorRefs => _errorRefs.Value;

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("implementationRef")]
    public XmlQualifiedName ImplementationRef { get; set; }
}