using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tResourceAssignmentExpression", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("resourceAssignmentExpression", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ResourceAssignmentExpression : BaseElement
    {
        [XmlElement("expression")]
        public Expression Expression { get; set; }
    }
}