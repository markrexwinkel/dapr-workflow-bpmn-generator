using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tTransaction", Namespace = Namespaces.Bpmn)]
[XmlRoot("transaction", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Transaction : SubProcess
{
    [XmlAttribute("method")]
    [DefaultValue("##Compensate")]
    public string Method { get; set; } = "##Compensate";
}