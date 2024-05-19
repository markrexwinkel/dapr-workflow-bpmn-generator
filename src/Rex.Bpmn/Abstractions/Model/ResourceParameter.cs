using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tResourceParameter", Namespace = Namespaces.Bpmn)]
[XmlRoot("resourceParameter", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ResourceParameter : BaseElement
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("type")]
    public XmlQualifiedName Type { get; set; }

    [XmlAttribute("isRequired")]
    public bool IsRequired { get; set; }
}