using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tExclusiveGateway", Namespace = Namespaces.Bpmn)]
[XmlRoot("exclusiveGateway", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ExclusiveGateway : Gateway, IDefaultSequence
{
    [XmlAttribute("default", DataType = "IDREF")]
    public string Default { get; set; }
}