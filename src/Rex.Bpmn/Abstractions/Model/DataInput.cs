using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tDataInput", Namespace = Namespaces.Bpmn)]
[XmlRoot("dataInput", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class DataInput : BaseElement
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