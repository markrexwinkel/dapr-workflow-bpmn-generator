using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tCorrelationSubscription", Namespace = Namespaces.Bpmn)]
[XmlRoot("correlationSubscription", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class CorrelationSubscription : BaseElement
{
    private readonly Lazy<Collection<CorrelationPropertyBinding>> _bindings = new();

    [XmlElement("correlationPropertyBinding")]
    public Collection<CorrelationPropertyBinding> Bindings => _bindings.Value;

    [XmlAttribute("correlationKeyRef")]
    public XmlQualifiedName CorrelationKeyRef { get; set; }
}