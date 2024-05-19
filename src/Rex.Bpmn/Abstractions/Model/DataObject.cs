using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tDataObject", Namespace = Namespaces.Bpmn)]
[XmlRoot("dataObject", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class DataObject : FlowElement
{
    [XmlElement("dataState")]
    public DataState DataState { get; set; }

    [XmlAttribute("itemSubjectRef")]
    public XmlQualifiedName ItemSubjectRef { get; set; }

    [XmlAttribute("isCollection")]
    public bool IsCollection { get; set; }
}