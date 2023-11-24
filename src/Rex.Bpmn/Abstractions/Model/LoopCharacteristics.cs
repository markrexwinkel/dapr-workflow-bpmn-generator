using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(StandardLoopCharacteristics))]
    [XmlInclude(typeof(MultiInstanceLoopCharacteristics))]
    [XmlType("tLoopCharacteristics", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("loopCharacteristics", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public abstract class LoopCharacteristics : BaseElement
    {
    }
}