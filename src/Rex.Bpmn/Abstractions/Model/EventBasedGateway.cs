using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tEventBasedGateway", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("eventBasedGateway", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class EventBasedGateway : Gateway
    {
        [XmlAttribute("instantiate")]
        public bool Instantiate { get; set; }

        [XmlAttribute("eventGatewayType")]
        public EventBasedGatewayType EventGatewayType { get; set; }
    }
}