using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType(Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("messageFlow", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
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
}