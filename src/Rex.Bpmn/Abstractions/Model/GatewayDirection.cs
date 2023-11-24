using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tGatewayDirection", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public enum GatewayDirection
    {
        Unspecified,
        Converging,
        Diverging,
        Mixed
    }
}