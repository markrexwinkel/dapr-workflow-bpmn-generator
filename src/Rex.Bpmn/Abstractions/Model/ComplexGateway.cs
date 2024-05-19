using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tComplexGateway", Namespace = Namespaces.Bpmn)]
[XmlRoot("complexGateway", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ComplexGateway : Gateway, IDefaultSequence
{
    [XmlElement("activationCondition")]
    public Expression ActivationCondition { get; set; }

    [XmlAttribute("default", DataType = "IDREF")]
    public string Default { get; set; }
}