using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(SequenceFlow))]
    [XmlInclude(typeof(FlowNode))]
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
    [XmlInclude(typeof(DataStoreReference))]
    [XmlInclude(typeof(DataObjectReference))]
    [XmlInclude(typeof(DataObject))]
    [XmlType("tFlowElement", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("flowElement", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public abstract class FlowElement : BaseElement
    {
        private readonly Lazy<Collection<XmlQualifiedName>> _categoryValueRefs = new Lazy<Collection<XmlQualifiedName>>();

        [XmlElement("auditing")]
        public Auditing Auditing { get; set; }

        [XmlElement("monitoring")]
        public Monitoring Monitoring { get; set; }

        [XmlElement("categoryValueRef")]
        public Collection<XmlQualifiedName> CategoryValueRefs => _categoryValueRefs.Value;

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}