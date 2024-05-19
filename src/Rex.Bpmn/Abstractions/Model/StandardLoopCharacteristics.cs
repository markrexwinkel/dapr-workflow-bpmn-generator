using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tStandardLoopCharacteristics", Namespace = Namespaces.Bpmn)]
[XmlRoot("standardLoopCharacteristics", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class StandardLoopCharacteristics : LoopCharacteristics
{
    [XmlElement("loopCondition")]
    public Expression LoopCondition { get; set; }

    [XmlAttribute("testBefore")]
    public bool TestBefore { get; set; }

    [XmlAttribute("loopMaximum")]
    public int LoopMaximum { get; set; }
}