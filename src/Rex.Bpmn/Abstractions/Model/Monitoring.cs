using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tMonitoring", Namespace = Namespaces.Bpmn)]
[XmlRoot("monitoring", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Monitoring : BaseElement
{
}