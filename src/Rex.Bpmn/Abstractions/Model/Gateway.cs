using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(ParallelGateway))]
[XmlInclude(typeof(InclusiveGateway))]
[XmlInclude(typeof(ExclusiveGateway))]
[XmlInclude(typeof(EventBasedGateway))]
[XmlInclude(typeof(ComplexGateway))]
[XmlType("tGateway", Namespace = Namespaces.Bpmn)]
public class Gateway : FlowNode
{
    [XmlAttribute("gatewayDirection")]
    [DefaultValue(GatewayDirection.Unspecified)]
    public GatewayDirection Direction { get; set; }
}