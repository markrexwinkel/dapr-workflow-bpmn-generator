using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tBoundaryEvent", Namespace = Namespaces.Bpmn)]
[XmlRoot("boundaryEvent", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class BoundaryEvent : CatchEvent
{
    [XmlAttribute("cancelActivity")]
    [DefaultValue(true)]
    public bool CancelActivity { get; set; } = true;

    [XmlAttribute("attachedToRef")]
    public XmlQualifiedName AttachedToRef { get; set; }
}