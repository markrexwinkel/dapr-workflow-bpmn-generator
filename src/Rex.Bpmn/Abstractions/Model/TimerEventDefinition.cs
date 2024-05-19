using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tTimerEventDefinition", Namespace = Namespaces.Bpmn)]
[XmlRoot("timerEventDefinition", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class TimerEventDefinition : EventDefinition
{
    [XmlElement("timeCycle", typeof(Expression))]
    [XmlElement("timeDate", typeof(Expression))]
    [XmlElement("timeDuration", typeof(Expression))]
    [XmlChoiceIdentifier("ItemElementName")]
    public Expression Item { get; set; }

    [XmlIgnore]
    public ItemChoiceType ItemElementName { get; set; }
}