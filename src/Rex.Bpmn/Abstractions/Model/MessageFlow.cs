using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.Bpmn)]
[XmlRoot("messageFlow", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class MessageFlow : BaseElement
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("sourceRef")]
    public XmlQualifiedName SourceRef { get; set; }

    [XmlAttribute("targetRef")]
    public XmlQualifiedName TargetRef { get; set; }

    [XmlAttribute("messageRef")]
    public XmlQualifiedName MessageRef { get; set; }
}