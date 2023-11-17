using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType(Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("textAnnotation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class TextAnnotation : Artifact
    {
        [XmlElement("text")]
        public XmlNode Text { get; set; }

        [XmlAttribute("textFormat")]
        [DefaultValue("text/plain")]
        public string TextFormat { get; set; } = "text/plain";
    }
}