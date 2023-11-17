using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tRelationshipDirection", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public enum RelationshipDirection
    {
        None,
        Forward,
        Backward,
        Both
    }
}