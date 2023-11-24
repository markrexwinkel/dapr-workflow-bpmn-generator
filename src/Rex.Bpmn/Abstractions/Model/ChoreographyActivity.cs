using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(CallChoreography))]
[XmlInclude(typeof(ChoreographyTask))]
[XmlInclude(typeof(SubChoreography))]
[XmlType("tChoreographyActivity", Namespace = Namespaces.Bpmn)]
[XmlRoot("choreographyActivity", Namespace = Namespaces.Bpmn, IsNullable = false)]
public abstract class ChoreographyActivity : FlowNode
{
    private readonly Lazy<Collection<XmlQualifiedName>> _participantRefs = new();
    private readonly Lazy<Collection<CorrelationKey>> _correlationKeys = new();

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