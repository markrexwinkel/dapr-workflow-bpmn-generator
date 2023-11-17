using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tGlobalManualTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("globalManualTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class GlobalManualTask : GlobalTask
    {
    }
}