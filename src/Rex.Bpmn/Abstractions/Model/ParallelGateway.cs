using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tParallelGateway", Namespace = Namespaces.Bpmn)]
[XmlRoot("parallelGateway", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ParallelGateway : Gateway
{
}