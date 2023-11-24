using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tProcessType", Namespace = Namespaces.Bpmn)]
public enum ProcessType
{
    None,
    Public,
    Private
}