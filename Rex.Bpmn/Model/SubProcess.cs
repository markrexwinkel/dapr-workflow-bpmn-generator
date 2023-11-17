using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlInclude(typeof(Transaction))]
    [XmlInclude(typeof(AdHocSubProcess))]
    [XmlType("tSubProcess", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("subProcess", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class SubProcess : Activity
    {
        private readonly Lazy<Collection<LaneSet>> _laneSets = new Lazy<Collection<LaneSet>>();
        private readonly Lazy<Collection<FlowElement>> _flowElements = new Lazy<Collection<FlowElement>>();
        private readonly Lazy<Collection<Artifact>> _artifacts = new Lazy<Collection<Artifact>>();

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

        [XmlAttribute("triggeredByEvent")]
        public bool TriggeredByEvent { get; set; }
    }
}