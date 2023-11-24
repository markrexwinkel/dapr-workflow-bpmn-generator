using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tAssociationDirection", Namespace = Namespaces.Bpmn)]
public enum AssociationDirection
{
    None,
    One,
    Both
}