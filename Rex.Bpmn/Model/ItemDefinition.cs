using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tItemDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("itemDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
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
}