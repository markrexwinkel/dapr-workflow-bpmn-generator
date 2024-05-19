using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tServiceTask", Namespace = Namespaces.Bpmn)]
[XmlRoot("serviceTask", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ServiceTask : Task
{
    [XmlAttribute("implementation")]
    [DefaultValue("##WebService")]
    public string Implementation { get; set; } = "##WebService";

    [XmlAttribute("operationRef")]
    public XmlQualifiedName OperationRef { get; set; }
}