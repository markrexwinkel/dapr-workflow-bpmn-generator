using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlInclude(typeof(CallChoreography))]
    [XmlInclude(typeof(ChoreographyTask))]
    [XmlInclude(typeof(SubChoreography))]
    [XmlType("tChoreographyActivity", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("choreographyActivity", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public abstract class ChoreographyActivity : FlowNode
    {
        private readonly Lazy<Collection<XmlQualifiedName>> _participantRefs = new Lazy<Collection<XmlQualifiedName>>();
        private readonly Lazy<Collection<CorrelationKey>> _correlationKeys = new Lazy<Collection<CorrelationKey>>();

        [XmlElement("participantRef")]
        public Collection<XmlQualifiedName> ParticipantRefs => _participantRefs.Value;

        [XmlElement("correlationKey")]
        public Collection<CorrelationKey> CorrelationKeys => _correlationKeys.Value;

        [XmlAttribute("initiatingParticipantRef")]
        public XmlQualifiedName InitiatingParticipantRef { get; set; }

        [XmlAttribute("loopType")]
        [DefaultValue(ChoreographyLoopType.None)]
        public ChoreographyLoopType LoopType { get; set; } = ChoreographyLoopType.None;
    }
}