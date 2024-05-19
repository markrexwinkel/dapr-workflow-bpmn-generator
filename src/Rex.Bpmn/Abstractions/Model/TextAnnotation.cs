using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.Bpmn)]
[XmlRoot("textAnnotation", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class TextAnnotation : Artifact
{
    [XmlElement("text")]
    public XmlNode Text { get; set; }

    [XmlAttribute("textFormat")]
    [DefaultValue("text/plain")]
    public string TextFormat { get; set; } = "text/plain";
}