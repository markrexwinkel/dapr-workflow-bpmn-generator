using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tGlobalChoreographyTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("globalChoreographyTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class GlobalChoreographyTask : Choreography
    {
        [XmlAttribute("initiatingParticipantRef")]
        public XmlQualifiedName InitiatingParticipantRef { get; set; }
    }
}