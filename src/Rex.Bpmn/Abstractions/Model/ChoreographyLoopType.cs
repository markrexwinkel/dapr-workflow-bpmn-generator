using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tChoreographyLoopType", Namespace = Namespaces.Bpmn)]
public enum ChoreographyLoopType
{
    None,
    Standard,
    MultiInstanceSequential,
    MultiInstanceParallel
}