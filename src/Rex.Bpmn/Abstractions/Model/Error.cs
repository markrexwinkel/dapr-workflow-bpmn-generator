using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tError", Namespace = Namespaces.Bpmn)]
[XmlRoot("error", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Error : RootElement
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("errorCode")]
    public string ErrorCode { get; set; }

    [XmlAttribute("structureRef")]
    public XmlQualifiedName StructureRef { get; set; }
}