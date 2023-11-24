using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(Expression))]
[XmlInclude(typeof(FormalExpression))]
[XmlType("tBaseElementWithMixedContent", Namespace = Namespaces.Bpmn)]
[XmlRoot("baseElementWithMixedContent", Namespace = Namespaces.Bpmn, IsNullable = false)]
public abstract class BaseElementWithMixedContent
{
    private readonly Lazy<Collection<XmlAttribute>> _extensionAttributes = new();
    private readonly Lazy<Collection<Documentation>> _documentation = new();

    [XmlAttribute("id", DataType = "ID")]
    public string Id { get; set; }
    [XmlAnyAttribute]
    public Collection<XmlAttribute> ExtensionAttributes => _extensionAttributes.Value;
    [XmlElement("documentation")]
    public Collection<Documentation> Documentation => _documentation.Value;
    [XmlElement("extensionElements")]
    public ExtensionElements ExtensionElements { get; set; }
    [XmlText]
    public Collection<string>  Text { get; set; }
}
