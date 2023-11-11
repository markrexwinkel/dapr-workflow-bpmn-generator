using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tStartEvent", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("startEvent", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class StartEvent : CatchEvent
    {
        [XmlAttribute("isInterupting")]
        [DefaultValue(true)]
        public bool IsInterrupting { get; set; } = true;
    }
}