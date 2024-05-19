using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tSignal", Namespace = Namespaces.Bpmn)]
[XmlRoot("signal", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class Signal : RootElement
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("structureRef")]
    public XmlQualifiedName StructureRef { get; set; }
}