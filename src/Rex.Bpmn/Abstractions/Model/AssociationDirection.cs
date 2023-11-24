using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tAssociationDirection", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public enum AssociationDirection
    {
        None,
        One,
        Both
    }
}