using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tCorrelationKey", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("correlationKey", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class CorrelationKey : BaseElement
    {
        private readonly Lazy<Collection<XmlQualifiedName>> _correlationPropertyRefs = new Lazy<Collection<XmlQualifiedName>>();

        [XmlElement("correlationPropertyRef")]
        public Collection<XmlQualifiedName> CorrelationPropertyRefs => _correlationPropertyRefs.Value;

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}