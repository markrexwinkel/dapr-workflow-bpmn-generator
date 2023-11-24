using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tCategoryValue", Namespace = Namespaces.Bpmn)]
[XmlRoot("categoryValue", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class CategoryValue : BaseElement
{
    [XmlAttribute("value")]
    public string Value { get; set; }
}