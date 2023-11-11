using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tEndPoint", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("endPoint", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class EndPoint : RootElement
    {
    }
}
