using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

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
[XmlType("tEventDefinition", Namespace = Namespaces.Bpmn)]
[XmlRoot("eventDefinition", Namespace = Namespaces.Bpmn, IsNullable = false)]
public abstract class EventDefinition : RootElement
{
}