using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.Bpmn, IncludeInSchema = false)]
public enum ItemChoiceType
{
    [XmlEnum("timeCycle")]
    TimeCycle,
    [XmlEnum("timeDate")]
    TimeDate,
    [XmlEnum("timeDuration")]
    TimeDuration
}