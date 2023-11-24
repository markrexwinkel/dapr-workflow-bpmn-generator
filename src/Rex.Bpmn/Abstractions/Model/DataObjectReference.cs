using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tDataObjectReference", Namespace = Namespaces.Bpmn)]
[XmlRoot("dataObjectReference", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class DataObjectReference : FlowElement
{
    [XmlElement("dataState")]
    public DataState DataState { get; set; }

    [XmlAttribute("itemSubjectName")]
    public XmlQualifiedName ItemSubjectRef { get; set; }

    [XmlAttribute("dataObjectRef", DataType = "IDREF")]
    public string DataObjectRef { get; set; }
}