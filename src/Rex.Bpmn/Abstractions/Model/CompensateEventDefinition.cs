using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tCompensateEventDefinition", Namespace = Namespaces.Bpmn)]
[XmlRoot("compensateEventDefinition", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class CompensateEventDefinition : EventDefinition
{
    [XmlAttribute("waitForCompletion")]
    public bool WaitForCompletion { get; set; }

    [XmlAttribute("activityRef")]
    public XmlQualifiedName ActivityRef { get; set; }
}