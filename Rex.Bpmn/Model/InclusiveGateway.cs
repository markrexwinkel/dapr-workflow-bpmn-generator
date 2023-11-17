using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tInclusiveGateway", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("inclusiveGateway", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class InclusiveGateway : Gateway, IDefaultSequence
    {
        [XmlAttribute("default", DataType = "IDREF")]
        public string Default { get; set; }
    }
}