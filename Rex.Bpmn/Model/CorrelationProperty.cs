using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tCorrelationProperty", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("correlationProperty", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class CorrelationProperty : RootElement
    {
        private readonly Lazy<Collection<CorrelationPropertyRetrievalExpression>> _retrievalExpressions = new Lazy<Collection<CorrelationPropertyRetrievalExpression>>();

        [XmlElement("correlationPropertyRetrievalExpression")]
        public Collection<CorrelationPropertyRetrievalExpression> RetrievalExpressions => _retrievalExpressions.Value;

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public XmlQualifiedName Type { get; set; }
    }
}