using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tAdHocSubProcess", Namespace = Namespaces.Bpmn)]
[XmlRoot("adHocSubProcess", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class AdHocSubProcess : SubProcess
{
    [XmlElement("completionCondition")]
    public Expression CompletionCondition { get; set; }

    [XmlAttribute("cancelRemainingInstances")]
    [DefaultValue(true)]
    public bool CancelRemainingInstances { get; set; } = true;

    [XmlAttribute("ordering")]
    public AdHocOrdering Ordering { get; set; }
}