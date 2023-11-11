using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tRendering", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("rendering", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Rendering : BaseElement
    {
    }
}