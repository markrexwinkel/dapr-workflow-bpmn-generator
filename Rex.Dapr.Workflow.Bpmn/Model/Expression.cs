using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlInclude(typeof(FormalExpression))]
    [XmlType("tExpression", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("expression", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Expression : BaseElementWithMixedContent
    {
    }
}