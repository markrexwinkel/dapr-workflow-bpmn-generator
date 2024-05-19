using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tInterface", Namespace = Namespaces.Bpmn)]
[XmlRoot("interface", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Interface : RootElement
{
    private readonly Lazy<Collection<Operation>> _operations = new();

    [XmlElement("operation")]
    public Collection<Operation> Operations => _operations.Value;

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("implementationRef")]
    public XmlQualifiedName ImplementationRef { get; set; }
}