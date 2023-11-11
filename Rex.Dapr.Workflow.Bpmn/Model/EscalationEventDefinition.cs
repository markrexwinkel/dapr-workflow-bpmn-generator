using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tEscalationEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("escalationEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class EscalationEventDefinition : EventDefinition
    {
        [XmlAttribute("escalationRef")]
        public XmlQualifiedName EscalationRef { get; set; }
    }
}