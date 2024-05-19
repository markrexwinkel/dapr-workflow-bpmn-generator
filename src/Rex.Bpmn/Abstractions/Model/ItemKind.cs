using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tItemKind", Namespace = Namespaces.Bpmn)]
public enum ItemKind
{
    Information,
    Physical
}