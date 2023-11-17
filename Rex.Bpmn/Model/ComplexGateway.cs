using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tComplexGateway", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("complexGateway", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ComplexGateway : Gateway, IDefaultSequence
    {
        [XmlElement("activationCondition")]
        public Expression ActivationCondition { get; set; }

        [XmlAttribute("default", DataType = "IDREF")]
        public string Default { get; set; }
    }
}