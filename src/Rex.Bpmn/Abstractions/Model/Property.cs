using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tProperty", Namespace = Namespaces.Bpmn)]
[XmlRoot("property", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Property : BaseElement
{
    [XmlElement("dataState")]
    public DataState DataState { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("itemSubjectRef")]
    public XmlQualifiedName ItemSubjectRef { get; set; }
}