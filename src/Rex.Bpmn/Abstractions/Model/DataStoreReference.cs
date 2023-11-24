using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tDataStoreReference", Namespace = Namespaces.Bpmn)]
[XmlRoot("dataStoreReference", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class DataStoreReference : FlowElement
{
    [XmlElement("dataState")]
    public DataState DataState { get; set; }

    [XmlAttribute("itemSubjectRef")]
    public XmlQualifiedName ItemSubjectRef { get; set; }

    [XmlAttribute("dataStoreRef")]
    public XmlQualifiedName DataStoreRef { get; set; }
}