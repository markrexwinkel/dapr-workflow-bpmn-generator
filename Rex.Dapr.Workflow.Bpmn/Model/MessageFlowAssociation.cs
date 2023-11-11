using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tMessageFlowAssociation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("messageFlowAssociation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class MessageFlowAssociation : BaseElement
    {
        [XmlAttribute("innerMessageFlowRef")]
        public XmlQualifiedName InnerMessageFlowRef { get; set; }

        [XmlAttribute("outerMessageFlowRef")]
        public XmlQualifiedName OuterMessageFlowRef { get; set; }
    }
}