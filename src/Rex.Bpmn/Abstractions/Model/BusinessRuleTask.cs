using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tBusinessRuleTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("businessRuleTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class BusinessRuleTask : Task
    {
        [XmlAttribute("implementation")]
        [DefaultValue("##unspecified")]
        public string Implementation { get; set; } = "##unspecified";
    }
}