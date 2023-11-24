using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tTimerEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("timerEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
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
}