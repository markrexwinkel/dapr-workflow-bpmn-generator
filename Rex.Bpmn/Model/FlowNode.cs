using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlInclude(typeof(Gateway))]
    [XmlInclude(typeof(ParallelGateway))]
    [XmlInclude(typeof(InclusiveGateway))]
    [XmlInclude(typeof(ExclusiveGateway))]
    [XmlInclude(typeof(EventBasedGateway))]
    [XmlInclude(typeof(ComplexGateway))]
    [XmlInclude(typeof(Event))]
    [XmlInclude(typeof(ThrowEvent))]
    [XmlInclude(typeof(IntermediateThrowEvent))]
    [XmlInclude(typeof(ImplicitThrowEvent))]
    [XmlInclude(typeof(EndEvent))]
    [XmlInclude(typeof(CatchEvent))]
    [XmlInclude(typeof(StartEvent))]
    [XmlInclude(typeof(IntermediateCatchEvent))]
    [XmlInclude(typeof(BoundaryEvent))]
    [XmlInclude(typeof(ChoreographyActivity))]
    [XmlInclude(typeof(SubChoreography))]
    [XmlInclude(typeof(ChoreographyTask))]
    [XmlInclude(typeof(CallChoreography))]
    [XmlInclude(typeof(Activity))]
    [XmlInclude(typeof(Task))]
    [XmlInclude(typeof(UserTask))]
    [XmlInclude(typeof(ServiceTask))]
    [XmlInclude(typeof(SendTask))]
    [XmlInclude(typeof(ScriptTask))]
    [XmlInclude(typeof(ReceiveTask))]
    [XmlInclude(typeof(ManualTask))]
    [XmlInclude(typeof(BusinessRuleTask))]
    [XmlInclude(typeof(SubProcess))]
    [XmlInclude(typeof(Transaction))]
    [XmlInclude(typeof(AdHocSubProcess))]
    [XmlInclude(typeof(CallActivity))]
    [XmlType("tFlowNode", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("flowNode", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public abstract class FlowNode : FlowElement
    {
        private readonly Lazy<Collection<XmlQualifiedName>> _incoming = new Lazy<Collection<XmlQualifiedName>>();
        private readonly Lazy<Collection<XmlQualifiedName>> _outgoing = new Lazy<Collection<XmlQualifiedName>>();

        [XmlElement("incoming")]
        public Collection<XmlQualifiedName> Incoming => _incoming.Value;

        [XmlElement("outgoing")]
        public Collection<XmlQualifiedName> Outgoing => _outgoing.Value;
    }
}