using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tGlobalConversation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("globalConversation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class GlobalConversation : Collaboration
    {
    }
}