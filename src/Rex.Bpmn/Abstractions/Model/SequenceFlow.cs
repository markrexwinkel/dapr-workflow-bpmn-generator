using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tSequenceFlow", Namespace = Namespaces.Bpmn)]
[XmlRoot("sequenceFlow", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class SequenceFlow : FlowElement
{
    [XmlElement("conditionExpression")]
    public Expression ConditionExpression { get; set; }

    [XmlAttribute("sourceRef", DataType = "IDREF")]
    public string SourceRef { get; set; }

    [XmlAttribute("targetRef", DataType = "IDREF")]
    public string TargetRef { get; set; }

    [XmlAttribute("isImmediate")]
    public bool IsImmediate { get; set; }
}