using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tSignalEventDefinition", Namespace = Namespaces.Bpmn)]
[XmlRoot("signalEventDefinition", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class SignalEventDefinition : EventDefinition
{
    [XmlAttribute("signalRef")]
    public XmlQualifiedName SignalRef { get; set; }
}