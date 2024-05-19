using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(TextAnnotation))]
[XmlInclude(typeof(Group))]
[XmlInclude(typeof(Association))]
[XmlType("tArtifact", Namespace = Namespaces.Bpmn)]
[XmlRoot("artifact", Namespace = Namespaces.Bpmn, IsNullable = false)]
public abstract class Artifact : BaseElement
{
}
