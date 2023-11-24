using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tRendering", Namespace = Namespaces.Bpmn)]
[XmlRoot("rendering", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Rendering : BaseElement
{
}