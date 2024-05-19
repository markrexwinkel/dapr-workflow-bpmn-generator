using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tEventBasedGateway", Namespace = Namespaces.Bpmn)]
[XmlRoot("eventBasedGateway", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class EventBasedGateway : Gateway
{
    [XmlAttribute("instantiate")]
    public bool Instantiate { get; set; }

    [XmlAttribute("eventGatewayType")]
    public EventBasedGatewayType EventGatewayType { get; set; }
}