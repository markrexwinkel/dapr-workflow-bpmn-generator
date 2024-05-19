using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tCorrelationProperty", Namespace = Namespaces.Bpmn)]
[XmlRoot("correlationProperty", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class CorrelationProperty : RootElement
{
    private readonly Lazy<Collection<CorrelationPropertyRetrievalExpression>> _retrievalExpressions = new();

    [XmlElement("correlationPropertyRetrievalExpression")]
    public Collection<CorrelationPropertyRetrievalExpression> RetrievalExpressions => _retrievalExpressions.Value;

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("type")]
    public XmlQualifiedName Type { get; set; }
}