using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(UserTask))]
    [XmlInclude(typeof(ServiceTask))]
    [XmlInclude(typeof(SendTask))]
    [XmlInclude(typeof(ScriptTask))]
    [XmlInclude(typeof(ReceiveTask))]
    [XmlInclude(typeof(ManualTask))]
    [XmlInclude(typeof(BusinessRuleTask))]
    [XmlType("tTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("task", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Task : Activity
    {
    }
}