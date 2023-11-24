using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(Process))]
    [XmlInclude(typeof(GlobalTask))]
    [XmlInclude(typeof(GlobalUserTask))]
    [XmlInclude(typeof(GlobalScriptTask))]
    [XmlInclude(typeof(GlobalManualTask))]
    [XmlInclude(typeof(GlobalBusinessRuleTask))]
    [XmlType("tCallableElement", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("callableElement", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class CallableElement : RootElement
    {
        private readonly Lazy<Collection<XmlQualifiedName>> _supportedInterfaceRefs = new Lazy<Collection<XmlQualifiedName>>();
        private readonly Lazy<Collection<InputOutputBinding>> _ioBindings = new Lazy<Collection<InputOutputBinding>>();

        [XmlElement("supportedInterfaceRef")]
        public Collection<XmlQualifiedName> SupportedInterfaceRefs => _supportedInterfaceRefs.Value;

        [XmlElement("ioSpecification")]
        public InputOutputSpecification IoSpecification { get; set; }

        [XmlElement("ioBinding")]
        public Collection<InputOutputBinding> IoBindings => _ioBindings.Value;

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}