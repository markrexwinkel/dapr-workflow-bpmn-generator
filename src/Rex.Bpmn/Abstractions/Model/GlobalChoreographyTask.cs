using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tGlobalChoreographyTask", Namespace = Namespaces.Bpmn)]
[XmlRoot("globalChoreographyTask", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class GlobalChoreographyTask : Choreography
{
    [XmlAttribute("initiatingParticipantRef")]
    public XmlQualifiedName InitiatingParticipantRef { get; set; }
}