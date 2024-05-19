using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(GlobalChoreographyTask))]
[XmlType("tChoreography", Namespace = Namespaces.Bpmn)]
[XmlRoot("choreography", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Choreography : Collaboration
{
    private readonly Lazy<Collection<FlowElement>> _flowElements = new();

    [XmlElement(typeof(SequenceFlow))]
    [XmlElement(typeof(FlowNode))]
    [XmlElement(typeof(Gateway))]
    [XmlElement(typeof(ParallelGateway))]
    [XmlElement(typeof(InclusiveGateway))]
    [XmlElement(typeof(ExclusiveGateway))]
    [XmlElement(typeof(EventBasedGateway))]
    [XmlElement(typeof(ComplexGateway))]
    [XmlElement(typeof(Event))]
    [XmlElement(typeof(ThrowEvent))]
    [XmlElement(typeof(IntermediateThrowEvent))]
    [XmlElement(typeof(ImplicitThrowEvent))]
    [XmlElement(typeof(EndEvent))]
    [XmlElement(typeof(CatchEvent))]
    [XmlElement(typeof(StartEvent))]
    [XmlElement(typeof(IntermediateCatchEvent))]
    [XmlElement(typeof(BoundaryEvent))]
    [XmlElement(typeof(ChoreographyActivity))]
    [XmlElement(typeof(SubChoreography))]
    [XmlElement(typeof(ChoreographyTask))]
    [XmlElement(typeof(CallChoreography))]
    [XmlElement(typeof(Activity))]
    [XmlElement(typeof(Task))]
    [XmlElement(typeof(UserTask))]
    [XmlElement(typeof(ServiceTask))]
    [XmlElement(typeof(SendTask))]
    [XmlElement(typeof(ScriptTask))]
    [XmlElement(typeof(ReceiveTask))]
    [XmlElement(typeof(ManualTask))]
    [XmlElement(typeof(BusinessRuleTask))]
    [XmlElement(typeof(SubProcess))]
    [XmlElement(typeof(Transaction))]
    [XmlElement(typeof(AdHocSubProcess))]
    [XmlElement(typeof(CallActivity))]
    [XmlElement(typeof(DataStoreReference))]
    [XmlElement(typeof(DataObjectReference))]
    [XmlElement(typeof(DataObject))]
    public Collection<FlowElement> FlowElements => _flowElements.Value;
}