using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tCancelEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("cancelEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class CancelEventDefinition : EventDefinition
    {
    }
}