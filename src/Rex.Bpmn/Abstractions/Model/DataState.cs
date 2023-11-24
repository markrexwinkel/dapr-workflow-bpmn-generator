using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tDataState", Namespace = Namespaces.Bpmn)]
[XmlRoot("dataState", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class DataState : BaseElement
{
    [XmlAttribute("name")]
    public string Name { get; set; }
}