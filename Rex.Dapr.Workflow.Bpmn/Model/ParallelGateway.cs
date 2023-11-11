using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tParallelGateway", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("parallelGateway", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ParallelGateway : Gateway
    {
    }
}