using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tItemKind", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public enum ItemKind
    {
        Information,
        Physical
    }
}