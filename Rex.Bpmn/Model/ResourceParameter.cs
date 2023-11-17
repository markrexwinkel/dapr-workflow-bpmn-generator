using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tResourceParameter", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("resourceParameter", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ResourceParameter : BaseElement
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public XmlQualifiedName Type { get; set; }

        [XmlAttribute("isRequired")]
        public bool IsRequired { get; set; }
    }
}