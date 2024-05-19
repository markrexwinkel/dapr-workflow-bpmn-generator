using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tEscalation", Namespace = Namespaces.Bpmn)]
[XmlRoot("escalation", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Escalation : RootElement
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("escalationCode")]
    public string EscalationCode { get; set; }

    [XmlAttribute("structureRef")]
    public XmlQualifiedName StructureRef { get; set; }
}