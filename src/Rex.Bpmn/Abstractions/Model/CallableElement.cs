using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(Process))]
[XmlInclude(typeof(GlobalTask))]
[XmlInclude(typeof(GlobalUserTask))]
[XmlInclude(typeof(GlobalScriptTask))]
[XmlInclude(typeof(GlobalManualTask))]
[XmlInclude(typeof(GlobalBusinessRuleTask))]
[XmlType("tCallableElement", Namespace = Namespaces.Bpmn)]
[XmlRoot("callableElement", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class CallableElement : RootElement
{
    private readonly Lazy<Collection<XmlQualifiedName>> _supportedInterfaceRefs = new();
    private readonly Lazy<Collection<InputOutputBinding>> _ioBindings = new();

    [XmlElement("supportedInterfaceRef")]
    public Collection<XmlQualifiedName> SupportedInterfaceRefs => _supportedInterfaceRefs.Value;

    [XmlElement("ioSpecification")]
    public InputOutputSpecification IoSpecification { get; set; }

    [XmlElement("ioBinding")]
    public Collection<InputOutputBinding> IoBindings => _ioBindings.Value;

    [XmlAttribute("name")]
    public string Name { get; set; }
}