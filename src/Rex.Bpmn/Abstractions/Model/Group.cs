using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tGroup", Namespace = Namespaces.Bpmn)]
[XmlRoot("group", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Group : Artifact
{
    public XmlQualifiedName CategoryValueRef { get; set; }
}