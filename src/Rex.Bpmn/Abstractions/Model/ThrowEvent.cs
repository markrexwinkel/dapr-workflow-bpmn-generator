using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(IntermediateThrowEvent))]
    [XmlInclude(typeof(ImplicitThrowEvent))]
    [XmlInclude(typeof(EndEvent))]
    [XmlType("tThrowEvent", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("throwEvent", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ThrowEvent : Event, IEventDefinitions
    {
        private readonly Lazy<Collection<DataInput>> _dataInputs = new Lazy<Collection<DataInput>>();
        private readonly Lazy<Collection<DataInputAssociation>> _dataInputAssociations = new Lazy<Collection<DataInputAssociation>>();
        private readonly Lazy<Collection<EventDefinition>> _eventDefinitions = new Lazy<Collection<EventDefinition>>();
        private readonly Lazy<Collection<XmlQualifiedName>> _eventDefinitionRefs = new Lazy<Collection<XmlQualifiedName>>();

        [XmlElement("dataInput")]
        public Collection<DataInput> DataInputs => _dataInputs.Value;

        [XmlElement("dataInputAssociation")]
        public Collection<DataInputAssociation> DataInputAssociations => _dataInputAssociations.Value;

        [XmlElement("inputSet")]
        public InputSet InputSet { get; set; }

        [XmlElement("timerEventDefinition", typeof(TimerEventDefinition))]
        [XmlElement("terminateEventDefinition", typeof(TerminateEventDefinition))]
        [XmlElement("signalEventDefinition", typeof(SignalEventDefinition))]
        [XmlElement("messageEventDefinition", typeof(MessageEventDefinition))]
        [XmlElement("linkEventDefinition", typeof(LinkEventDefinition))]
        [XmlElement("escalationEventDefinition", typeof(EscalationEventDefinition))]
        [XmlElement("errorEventDefinition", typeof(ErrorEventDefinition))]
        [XmlElement("conditionalEventDefinition", typeof(ConditionalEventDefinition))]
        [XmlElement("compensateEventDefinition", typeof(CompensateEventDefinition))]
        [XmlElement("cancelEventDefinition", typeof(CancelEventDefinition))]
        public Collection<EventDefinition> EventDefinitions => _eventDefinitions.Value;

        [XmlElement("eventDefinitionRef")]
        public Collection<XmlQualifiedName> EventDefinitionRefs => _eventDefinitionRefs.Value;
    }
}