using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tMessage", Namespace = Namespaces.Bpmn)]
[XmlRoot("message", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Message : RootElement
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("itemRef")]
    public XmlQualifiedName ItemRef { get; set; }
}