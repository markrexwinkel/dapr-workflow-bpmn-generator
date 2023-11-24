using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tComplexBehaviorDefinition", Namespace = Namespaces.Bpmn)]
[XmlRoot("complexBehaviorDefinition", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ComplexBehaviorDefinition : BaseElement
{
    [XmlElement("condition")]
    public FormalExpression Condition { get; set; }

    [XmlElement("event")]
    public ImplicitThrowEvent Event { get; set; }
}