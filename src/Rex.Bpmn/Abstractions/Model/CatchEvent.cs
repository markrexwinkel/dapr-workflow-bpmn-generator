using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(StartEvent))]
[XmlInclude(typeof(IntermediateCatchEvent))]
[XmlInclude(typeof(BoundaryEvent))]
[XmlType("tCatchEvent", Namespace = Namespaces.Bpmn)]
[XmlRoot("catchEvent", Namespace = Namespaces.Bpmn, IsNullable = false)]
public abstract class CatchEvent : Event, IEventDefinitions
{
    private readonly Lazy<Collection<DataOutput>> _dataOutputs = new();
    private readonly Lazy<Collection<DataOutputAssociation>> _dataOutputAssociations = new();
    private readonly Lazy<Collection<EventDefinition>> _eventDefinitions = new();
    private readonly Lazy<Collection<XmlQualifiedName>> _eventDefinitionRefs = new();

    [XmlElement("dataOutput")]
    public Collection<DataOutput> DataOutputs => _dataOutputs.Value;

    [XmlElement("dataOutputAssociation")]
    public Collection<DataOutputAssociation> DataOutputAssociations => _dataOutputAssociations.Value;

    [XmlElement("outputSet")]
    public OutputSet OutputSet { get; set; }

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

    [XmlAttribute("parallelMulitple")]
    public bool ParallelMultiple { get; set; }
}