using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tConditionalEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("conditionalEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ConditionalEventDefinition : EventDefinition
    {
        [XmlElement("condition")]
        public Expression Condition { get; set; }
    }
}