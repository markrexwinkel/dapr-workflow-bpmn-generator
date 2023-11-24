using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(HumanPerformer))]
    [XmlInclude(typeof(PotentialOwner))]
    [XmlType("tPerformer", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("performer", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Performer : ResourceRole
    {
    }
}