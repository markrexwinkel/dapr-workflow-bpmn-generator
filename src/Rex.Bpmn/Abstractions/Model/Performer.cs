using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(HumanPerformer))]
[XmlInclude(typeof(PotentialOwner))]
[XmlType("tPerformer", Namespace = Namespaces.Bpmn)]
[XmlRoot("performer", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Performer : ResourceRole
{
}