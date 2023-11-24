namespace Rex.Bpmn.Abstractions.Model
{
    [System.Xml.Serialization.XmlType("tEventBasedGatewayType", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public enum EventBasedGatewayType
    {
        Exclusive,
        Parallel
    }
}