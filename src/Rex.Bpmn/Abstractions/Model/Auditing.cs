using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tAuditing", Namespace = Namespaces.Bpmn)]
[XmlRoot("auditing", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Auditing : BaseElement
{
}
