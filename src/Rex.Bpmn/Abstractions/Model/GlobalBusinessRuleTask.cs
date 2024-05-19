using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tGlobalBusinessRuleTask", Namespace = Namespaces.Bpmn)]
[XmlRoot("globalBusinessRuleTask", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class GlobalBusinessRuleTask : GlobalTask
{
    [XmlAttribute("implementation")]
    [DefaultValue("##unspecified")]
    public string Implementation { get; set; } = "##unspecified";
}