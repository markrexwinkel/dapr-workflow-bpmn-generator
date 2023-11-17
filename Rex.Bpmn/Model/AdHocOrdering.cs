using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tAdHocOrdering", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public enum AdHocOrdering
    {
        Parallel,
        Sequential
    }
}