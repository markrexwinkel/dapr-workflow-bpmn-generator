using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tBusinessRuleTask", Namespace = Namespaces.Bpmn)]
[XmlRoot("businessRuleTask", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class BusinessRuleTask : Task
{
    [XmlAttribute("implementation")]
    [DefaultValue("##unspecified")]
    public string Implementation { get; set; } = "##unspecified";
}