using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tDataOutput", Namespace = Namespaces.Bpmn)]
[XmlRoot("dataOutput", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class DataOutput : BaseElement
{
    [XmlElement("dataState")]
    public DataState DataState { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("itemSubjectRef")]
    public XmlQualifiedName ItemSubjectRef { get; set; }

    [XmlAttribute("isCollection")]
    public bool IsCollection { get; set; }
}