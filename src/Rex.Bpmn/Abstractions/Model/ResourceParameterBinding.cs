using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tResourceParameterParameterBinding", Namespace = Namespaces.Bpmn)]
[XmlRoot("resourceParameterBinding", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ResourceParameterBinding : BaseElement
{
    [XmlElement("expression")]
    public Expression Expression { get; set; }

    [XmlAttribute("parameterRef")]
    public XmlQualifiedName ParameterRef { get; set; }
}