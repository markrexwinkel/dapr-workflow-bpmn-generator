using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tManualTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("manualTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ManualTask : Task
    {
    }
}