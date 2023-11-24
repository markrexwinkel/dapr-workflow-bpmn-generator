using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(PotentialOwner))]
    [XmlType("tHumanPerformer", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("humanPerformer", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class HumanPerformer : Performer
    {
    }
}