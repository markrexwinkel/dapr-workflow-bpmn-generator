using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tItemDefinition", Namespace = Namespaces.Bpmn)]
[XmlRoot("itemDefinition", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ItemDefinition : RootElement
{
    [XmlAttribute("structureRef")]
    public XmlQualifiedName StructureRef { get; set; }

    [XmlAttribute("isCollection")]
    public bool IsCollection { get; set; }

    [XmlAttribute("itemKind")]
    [DefaultValue(ItemKind.Information)]
    public ItemKind ItemKind { get; set; } = ItemKind.Information;
}