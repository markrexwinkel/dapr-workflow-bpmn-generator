using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tMultiInstanceFlowCondition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public enum MultiInstanceFlowCondition
    {
        None,
        One,
        All,
        Complex
    }
}