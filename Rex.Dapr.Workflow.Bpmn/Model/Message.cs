using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tMessage", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("message", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Message : RootElement
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("itemRef")]
        public XmlQualifiedName ItemRef { get; set; }
    }
}