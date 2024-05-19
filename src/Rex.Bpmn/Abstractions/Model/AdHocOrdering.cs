using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tAdHocOrdering", Namespace = Namespaces.Bpmn)]
public enum AdHocOrdering
{
    Parallel,
    Sequential
}