using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(UserTask))]
[XmlInclude(typeof(ServiceTask))]
[XmlInclude(typeof(SendTask))]
[XmlInclude(typeof(ScriptTask))]
[XmlInclude(typeof(ReceiveTask))]
[XmlInclude(typeof(ManualTask))]
[XmlInclude(typeof(BusinessRuleTask))]
[XmlType("tTask", Namespace = Namespaces.Bpmn)]
[XmlRoot("task", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Task : Activity
{
}