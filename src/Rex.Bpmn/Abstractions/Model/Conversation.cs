using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tConversation", Namespace = Namespaces.Bpmn)]
[XmlRoot("conversation", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Conversation : ConversationNode
{
}