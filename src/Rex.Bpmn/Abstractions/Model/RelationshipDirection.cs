using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tRelationshipDirection", Namespace = Namespaces.Bpmn)]
public enum RelationshipDirection
{
    None,
    Forward,
    Backward,
    Both
}