using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tSendTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("sendTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class SendTask : Task
    {
        [XmlAttribute("implementation")]
        [DefaultValue("##WebService")]
        public string Implementation { get; set; } = "##WebService";

        [XmlAttribute("messageRef")]
        public XmlQualifiedName MessageRef { get; set; }

        [XmlAttribute("operationRef")]
        public XmlQualifiedName OperationRef { get; set; }
    }
}