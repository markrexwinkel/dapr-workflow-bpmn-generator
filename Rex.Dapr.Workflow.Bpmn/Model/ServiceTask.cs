using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tServiceTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("serviceTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ServiceTask : Task
    {
        [XmlAttribute("implementation")]
        [DefaultValue("##WebService")]
        public string Implementation { get; set; } = "##WebService";

        [XmlAttribute("operationRef")]
        public XmlQualifiedName OperationRef { get; set; }
    }
}