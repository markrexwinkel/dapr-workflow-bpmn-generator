using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType(Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IncludeInSchema = false)]
    public enum ItemChoiceType
    {
        [XmlEnum("timeCycle")]
        TimeCycle,
        [XmlEnum("timeDate")]
        TimeDate,
        [XmlEnum("timeDuration")]
        TimeDuration
    }
}