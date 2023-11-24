using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(PotentialOwner))]
[XmlType("tHumanPerformer", Namespace = Namespaces.Bpmn)]
[XmlRoot("humanPerformer", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class HumanPerformer : Performer
{
}