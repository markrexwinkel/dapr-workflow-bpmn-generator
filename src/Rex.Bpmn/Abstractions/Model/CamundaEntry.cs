using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.CamundaBpmn)]
[XmlRoot(Namespace = Namespaces.CamundaBpmn, IsNullable = false)]
public class CamundaEntry
{
    [XmlAttribute("key")]
    public string Key { get; set; }

    [XmlText]
    public string Value { get; set; }
}
