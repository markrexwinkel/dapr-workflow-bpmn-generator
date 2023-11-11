using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tExclusiveGateway", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("exclusiveGateway", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ExclusiveGateway : Gateway, IDefaultSequence
    {
        [XmlAttribute("default", DataType = "IDREF")]
        public string Default { get; set; }
    }
}