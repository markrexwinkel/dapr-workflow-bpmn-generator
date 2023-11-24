using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tMonitoring", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("monitoring", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Monitoring : BaseElement
    {
    }
}