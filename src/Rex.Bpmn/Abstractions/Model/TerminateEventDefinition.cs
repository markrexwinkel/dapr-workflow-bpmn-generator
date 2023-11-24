using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tTerminateEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("terminateEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class TerminateEventDefinition : EventDefinition
    {
    }
}