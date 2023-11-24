using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tSignalEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("signalEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class SignalEventDefinition : EventDefinition
    {
        [XmlAttribute("signalRef")]
        public XmlQualifiedName SignalRef { get; set; }
    }
}