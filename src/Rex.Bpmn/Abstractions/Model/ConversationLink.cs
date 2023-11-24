using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tConversationLink", Namespace=Namespaces.Bpmn)]
[XmlRoot("conversationLink", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ConversationLink : BaseElement
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("sourceRef")]
    public XmlQualifiedName SourceRef { get; set; }

    [XmlAttribute("targetRef")]
    public XmlQualifiedName TargetRef { get; set; }
}