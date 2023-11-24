using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tStandardLoopCharacteristics", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("standardLoopCharacteristics", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class StandardLoopCharacteristics : LoopCharacteristics
    {
        [XmlElement("loopCondition")]
        public Expression LoopCondition { get; set; }

        [XmlAttribute("testBefore")]
        public bool TestBefore { get; set; }

        [XmlAttribute("loopMaximum")]
        public int LoopMaximum { get; set; }
    }
}