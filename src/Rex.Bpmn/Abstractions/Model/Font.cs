using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.BpmnDC)]
[XmlRoot(Namespace = Namespaces.BpmnDC, IsNullable = false)]
public class Font
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("size")]
    public double Size { get; set; }

    [XmlIgnore]
    public bool SizeSpecified { get; set; }

    [XmlAttribute("isBold")]
    public bool IsBold { get; set; }

    [XmlAttribute("isItalic")]
    public bool IsItalic { get; set; }

    [XmlAttribute("isUnderline")]
    public bool IsUnderline { get; set; }

    [XmlAttribute("isStrikeThrough")]
    public bool IsStrikeThrough { get; set; }


}