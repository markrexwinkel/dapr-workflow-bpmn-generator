using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.Bpmn)]
[XmlRoot("import", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Import
{
    [XmlAttribute("namespace", DataType = "anyURI")]
    public string Namespace { get; set; }

    [XmlAttribute("location")]
    public string Location { get; set; }

    [XmlAttribute("importType", DataType = "anyURI")]
    public string ImportType { get; set; }
}