namespace Rex.Bpmn.Abstractions.Model;

[System.Xml.Serialization.XmlType("tEventBasedGatewayType", Namespace = Namespaces.Bpmn)]
public enum EventBasedGatewayType
{
    Exclusive,
    Parallel
}