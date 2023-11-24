using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tEndPoint", Namespace = Namespaces.Bpmn)]
[XmlRoot("endPoint", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class EndPoint : RootElement
{
}
