using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tDataInputAssociation", Namespace = Namespaces.Bpmn)]
[XmlRoot("dataInputAssociation", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class DataInputAssociation : DataAssociation
{
}