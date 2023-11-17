using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlInclude(typeof(TimerEventDefinition))]
    [XmlInclude(typeof(TerminateEventDefinition))]
    [XmlInclude(typeof(SignalEventDefinition))]
    [XmlInclude(typeof(MessageEventDefinition))]
    [XmlInclude(typeof(LinkEventDefinition))]
    [XmlInclude(typeof(EscalationEventDefinition))]
    [XmlInclude(typeof(ErrorEventDefinition))]
    [XmlInclude(typeof(ConditionalEventDefinition))]
    [XmlInclude(typeof(CompensateEventDefinition))]
    [XmlInclude(typeof(CancelEventDefinition))]
    [XmlType("tEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("eventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public abstract class EventDefinition : RootElement
    {
    }
}