using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tConversationLink", Namespace="http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("conversationLink", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ConversationLink : BaseElement
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("sourceRef")]
        public XmlQualifiedName SourceRef { get; set; }

        [XmlAttribute("targetRef")]
        public XmlQualifiedName TargetRef { get; set; }
    }
}