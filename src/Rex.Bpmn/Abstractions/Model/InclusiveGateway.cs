using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tInclusiveGateway", Namespace = Namespaces.Bpmn)]
[XmlRoot("inclusiveGateway", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class InclusiveGateway : Gateway, IDefaultSequence
{
    [XmlAttribute("default", DataType = "IDREF")]
    public string Default { get; set; }
}