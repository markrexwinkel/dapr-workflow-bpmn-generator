using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlInclude(typeof(ParallelGateway))]
    [XmlInclude(typeof(InclusiveGateway))]
    [XmlInclude(typeof(ExclusiveGateway))]
    [XmlInclude(typeof(EventBasedGateway))]
    [XmlInclude(typeof(ComplexGateway))]
    [XmlType("tGateway", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public class Gateway : FlowNode
    {
        [XmlAttribute("gatewayDirection")]
        [DefaultValue(GatewayDirection.Unspecified)]
        public GatewayDirection Direction { get; set; }
    }
}