using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tComplexBehaviorDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("complexBehaviorDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ComplexBehaviorDefinition : BaseElement
    {
        [XmlElement("condition")]
        public FormalExpression Condition { get; set; }

        [XmlElement("event")]
        public ImplicitThrowEvent Event { get; set; }
    }
}