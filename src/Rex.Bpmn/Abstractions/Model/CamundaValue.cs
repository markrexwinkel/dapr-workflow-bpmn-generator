using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.CamundaBpmn)]
[XmlRoot(Namespace = Namespaces.CamundaBpmn, IsNullable = false)]
public class CamundaValue
{
    [XmlText]
    public string Value { get; set; }
}
