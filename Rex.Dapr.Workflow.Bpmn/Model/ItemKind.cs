using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tItemKind", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public enum ItemKind
    {
        Information,
        Physical
    }
}