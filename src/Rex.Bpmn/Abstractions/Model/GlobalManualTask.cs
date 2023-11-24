using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tGlobalManualTask", Namespace = Namespaces.Bpmn)]
[XmlRoot("globalManualTask", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class GlobalManualTask : GlobalTask
{
}