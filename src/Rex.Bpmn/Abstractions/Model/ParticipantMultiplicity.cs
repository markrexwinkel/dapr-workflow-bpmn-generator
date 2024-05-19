using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tParticipantMultiplicity", Namespace = Namespaces.Bpmn)]
[XmlRoot("participantMultiplicity", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ParticipantMultiplicity : BaseElement
{
    [XmlAttribute("minimum")]
    [DefaultValue(0)]
    public int Minimum { get; set; } = 0;

    [XmlAttribute("maximum")]
    [DefaultValue(1)]
    public int Maximum { get; set; } = 1;
}