using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(TextAnnotation))]
    [XmlInclude(typeof(Group))]
    [XmlInclude(typeof(Association))]
    [XmlType("tArtifact", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("artifact", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public abstract class Artifact : BaseElement
    {
    }
}
