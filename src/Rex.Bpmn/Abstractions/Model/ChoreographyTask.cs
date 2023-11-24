using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tChoreographyTask", Namespace = Namespaces.Bpmn)]
[XmlRoot("choreographyTask", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ChoreographyTask : ChoreographyActivity
{
    private readonly Lazy<Collection<XmlQualifiedName>> _messageFlowRefs = new();

    [XmlElement("messageFlowRef")]
    public Collection<XmlQualifiedName> MessageFlowRefs => _messageFlowRefs.Value;
}