using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tGlobalConversation", Namespace = Namespaces.Bpmn)]
[XmlRoot("globalConversation", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class GlobalConversation : Collaboration
{
}