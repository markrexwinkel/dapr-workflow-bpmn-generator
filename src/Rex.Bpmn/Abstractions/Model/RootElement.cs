using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(Signal))]
    [XmlInclude(typeof(Resource))]
    [XmlInclude(typeof(PartnerRole))]
    [XmlInclude(typeof(PartnerEntity))]
    [XmlInclude(typeof(Message))]
    [XmlInclude(typeof(ItemDefinition))]
    [XmlInclude(typeof(Interface))]
    [XmlInclude(typeof(EventDefinition))]
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
    [XmlInclude(typeof(Escalation))]
    [XmlInclude(typeof(Error))]
    [XmlInclude(typeof(EndPoint))]
    [XmlInclude(typeof(DataStore))]
    [XmlInclude(typeof(CorrelationProperty))]
    [XmlInclude(typeof(Collaboration))]
    [XmlInclude(typeof(GlobalConversation))]
    [XmlInclude(typeof(Choreography))]
    [XmlInclude(typeof(GlobalChoreographyTask))]
    [XmlInclude(typeof(Category))]
    [XmlInclude(typeof(CallableElement))]
    [XmlInclude(typeof(Process))]
    [XmlInclude(typeof(GlobalTask))]
    [XmlInclude(typeof(GlobalUserTask))]
    [XmlInclude(typeof(GlobalScriptTask))]
    [XmlInclude(typeof(GlobalManualTask))]
    [XmlInclude(typeof(GlobalBusinessRuleTask))]
    [XmlType("tRootElement", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("rootElement", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public abstract class RootElement : BaseElement
    {

    }
}