using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tCorrelationSubscription", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("correlationSubscription", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class CorrelationSubscription : BaseElement
    {
        private readonly Lazy<Collection<CorrelationPropertyBinding>> _bindings = new Lazy<Collection<CorrelationPropertyBinding>>();

        [XmlElement("correlationPropertyBinding")]
        public Collection<CorrelationPropertyBinding> Bindings => _bindings.Value;

        [XmlAttribute("correlationKeyRef")]
        public XmlQualifiedName CorrelationKeyRef { get; set; }
    }
}