using System.Xml.Serialization;

namespace Rex.Dapr.Workflow.Bpmn.Model;

[XmlType(Namespace = Namespaces.DaprBpmn)]
[XmlRoot(Namespace = Namespaces.DaprBpmn, IsNullable = false)]
public class DaprParameter
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("type")]
    public string Type { get; set; }

    [XmlAttribute("ref")]
    public string Ref { get; set; }

    [XmlAttribute("scope")]
    public DaprParameterScope Scope { get; set; } = DaprParameterScope.Local;

    [XmlText]
    public string Value { get; set; }
}
