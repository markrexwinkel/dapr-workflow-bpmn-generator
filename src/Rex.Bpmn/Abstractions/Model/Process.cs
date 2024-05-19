using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tProcess", Namespace = Namespaces.Bpmn)]
[XmlRoot("process", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Process : CallableElement, IFlowElements
{
    private readonly Lazy<Collection<Property>> _properties = new();
    private readonly Lazy<Collection<LaneSet>> _laneSets = new();
    private readonly Lazy<Collection<FlowElement>> _flowElements = new();
    private readonly Lazy<Collection<Artifact>> _artifacts = new();
    private readonly Lazy<Collection<ResourceRole>> _resourceRoles = new();
    private readonly Lazy<Collection<CorrelationSubscription>> _correlationSubscriptions = new();
    private readonly Lazy<Collection<XmlQualifiedName>> _supports = new();

    [XmlElement("auditing")]
    public Auditing Auditing { get; set; }

    [XmlElement("monitoring")]
    public Monitoring Monitoring { get; set; }

    [XmlElement("property")]
    public Collection<Property> Properties => _properties.Value;

    [XmlElement("laneSet")]
    public Collection<LaneSet> LaneSets => _laneSets.Value;

    [XmlElement("sequenceFlow", typeof(SequenceFlow))]
    [XmlElement("flowNode", typeof(FlowNode))]
    [XmlElement("gateway", typeof(Gateway))]
    [XmlElement("parallelGateway", typeof(ParallelGateway))]
    [XmlElement("inclusiveGateway", typeof(InclusiveGateway))]
    [XmlElement("exclusiveGateway", typeof(ExclusiveGateway))]
    [XmlElement("eventBasedGateway", typeof(EventBasedGateway))]
    [XmlElement("complexGateway", typeof(ComplexGateway))]
    [XmlElement("event", typeof(Event))]
    [XmlElement("throwEvent", typeof(ThrowEvent))]
    [XmlElement("intermediateThrowEvent", typeof(IntermediateThrowEvent))]
    [XmlElement("implicitThrowEvent", typeof(ImplicitThrowEvent))]
    [XmlElement("endEvent", typeof(EndEvent))]
    [XmlElement("catchEvent", typeof(CatchEvent))]
    [XmlElement("startEvent", typeof(StartEvent))]
    [XmlElement("intermediateCatchEvent", typeof(IntermediateCatchEvent))]
    [XmlElement("boundaryEvent", typeof(BoundaryEvent))]
    [XmlElement("choreographyActivity", typeof(ChoreographyActivity))]
    [XmlElement("subChoreography", typeof(SubChoreography))]
    [XmlElement("choreographyTask", typeof(ChoreographyTask))]
    [XmlElement("callChoreography", typeof(CallChoreography))]
    [XmlElement("activity", typeof(Activity))]
    [XmlElement("task", typeof(Task))]
    [XmlElement("userTask", typeof(UserTask))]
    [XmlElement("serviceTask", typeof(ServiceTask))]
    [XmlElement("sendTask", typeof(SendTask))]
    [XmlElement("scriptTask", typeof(ScriptTask))]
    [XmlElement("receiveTask", typeof(ReceiveTask))]
    [XmlElement("manualTask", typeof(ManualTask))]
    [XmlElement("businessRuleTask", typeof(BusinessRuleTask))]
    [XmlElement("subProcess", typeof(SubProcess))]
    [XmlElement("transaction", typeof(Transaction))]
    [XmlElement("adHocSubProcess", typeof(AdHocSubProcess))]
    [XmlElement("callActivity", typeof(CallActivity))]
    [XmlElement("dataStoreReference", typeof(DataStoreReference))]
    [XmlElement("dataObjectReference", typeof(DataObjectReference))]
    [XmlElement("dataObject", typeof(DataObject))]
    public Collection<FlowElement> FlowElements => _flowElements.Value;

    [XmlElement("textAnnotation", typeof(TextAnnotation))]
    [XmlElement("group", typeof(Group))]
    [XmlElement("association", typeof(Association))]
    public Collection<Artifact> Artifacts => _artifacts.Value;

    [XmlElement("resourceRole")]
    public Collection<ResourceRole> ResourceRoles => _resourceRoles.Value;

    [XmlElement("correlationSubscription")]
    public Collection<CorrelationSubscription> CorrelationSubscriptions => _correlationSubscriptions.Value;

    [XmlElement("supports")]
    public Collection<XmlQualifiedName> Supports => _supports.Value;

    [XmlAttribute("processType")]
    [DefaultValue(ProcessType.None)]
    public ProcessType ProcessType { get; set; } = ProcessType.None;

    [XmlAttribute("isClosed")]
    public bool IsClosed { get; set; }

    [XmlAttribute("isExecutable")]
    public bool IsExecutable { get; set; }

    [XmlAttribute("definitionCollaborationRef")]
    public XmlQualifiedName DefinitionCollaborationRef { get; set; }
}