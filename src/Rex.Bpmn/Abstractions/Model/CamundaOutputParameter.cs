using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.CamundaBpmn)]
[XmlRoot(Namespace = Namespaces.CamundaBpmn, IsNullable = false)]
public class CamundaOutputParameter
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlText]
    public string Value { get; set; }

    [XmlElement("map")]
    public CamundaMap Map { get; set; }

    [XmlElement("list")]
    public CamundaList List { get; set; }

    [XmlElement("script")]
    public CamundaScript Script { get; set; }
}
