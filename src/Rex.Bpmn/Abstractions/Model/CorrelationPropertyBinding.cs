using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tCorrelationPropertyBinding", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("correlationPropertyBinding", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class CorrelationPropertyBinding : BaseElement
    {
        [XmlElement("dataPath")]
        public FormalExpression DataPath { get; set; }

        [XmlAttribute("correlationPropertyRef")]
        public XmlQualifiedName CorrelationPropertyRef { get; set; }
    }
}