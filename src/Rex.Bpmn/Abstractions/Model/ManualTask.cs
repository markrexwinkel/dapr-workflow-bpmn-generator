using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tManualTask", Namespace = Namespaces.Bpmn)]
[XmlRoot("manualTask", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ManualTask : Task
{
}