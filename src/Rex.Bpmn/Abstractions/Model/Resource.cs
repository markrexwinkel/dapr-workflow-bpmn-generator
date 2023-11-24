using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tResource", Namespace = Namespaces.Bpmn)]
[XmlRoot("resource", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Resource : RootElement
{
    private readonly Lazy<Collection<ResourceParameter>> _resourceParameters = new();

    [XmlElement("resourceParameter")]
    public Collection<ResourceParameter> ResourceParameters => _resourceParameters.Value;

    [XmlAttribute("name")]
    public string Name { get; set; }
}