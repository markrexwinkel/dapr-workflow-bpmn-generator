using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tPotentialOwner", Namespace = Namespaces.Bpmn)]
[XmlRoot("potentialOwner", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class PotentialOwner : HumanPerformer
{
}