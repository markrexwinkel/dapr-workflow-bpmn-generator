using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tSendTask", Namespace = Namespaces.Bpmn)]
[XmlRoot("sendTask", Namespace = Namespaces.Bpmn, IsNullable = false)]
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