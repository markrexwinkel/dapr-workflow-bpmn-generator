using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tDataOutputAssociation", Namespace = Namespaces.Bpmn)]
[XmlRoot("dataOutputAssociation", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class DataOutputAssociation : DataAssociation
{
}