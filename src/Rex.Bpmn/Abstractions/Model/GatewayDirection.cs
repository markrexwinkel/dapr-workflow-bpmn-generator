using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tGatewayDirection", Namespace = Namespaces.Bpmn)]
public enum GatewayDirection
{
    Unspecified,
    Converging,
    Diverging,
    Mixed
}