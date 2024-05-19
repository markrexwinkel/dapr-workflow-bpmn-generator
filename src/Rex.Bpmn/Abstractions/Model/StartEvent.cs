using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tStartEvent", Namespace = Namespaces.Bpmn)]
[XmlRoot("startEvent", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class StartEvent : CatchEvent
{
    [XmlAttribute("isInterupting")]
    [DefaultValue(true)]
    public bool IsInterrupting { get; set; } = true;
}