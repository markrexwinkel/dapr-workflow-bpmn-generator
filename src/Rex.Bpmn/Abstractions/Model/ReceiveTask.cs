using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tReceiveTask", Namespace = Namespaces.Bpmn)]
[XmlRoot("receiveTask", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ReceiveTask : Task
{
    [XmlAttribute("implementation")]
    [DefaultValue("##WebService")]
    public string Implementation { get; set; } = "##WebService";

    [XmlAttribute("instantiate")]
    public bool Instantiate { get; set; }

    [XmlAttribute("messageRef")]
    public XmlQualifiedName MessageRef { get; set; }

    [XmlAttribute("operationRef")]
    public XmlQualifiedName OperationRef { get; set; }
}