using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tPotentialOwner", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("potentialOwner", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class PotentialOwner : HumanPerformer
    {
    }
}