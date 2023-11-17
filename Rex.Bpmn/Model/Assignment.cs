using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tAssignment", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("assignment", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Assignment : BaseElement
    {
        [XmlElement("from")]
        public Expression From { get; set; }
        [XmlElement("to")]
        public Expression To { get; set; }
    }
}
