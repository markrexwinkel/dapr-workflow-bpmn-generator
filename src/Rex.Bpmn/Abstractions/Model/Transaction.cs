using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tTransaction", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("transaction", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Transaction : SubProcess
    {
        [XmlAttribute("method")]
        [DefaultValue("##Compensate")]
        public string Method { get; set; } = "##Compensate";
    }
}