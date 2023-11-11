using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tDefinitions", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("definitions", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Definitions
    {
        private readonly Lazy<Collection<XmlAttribute>> _extensionAttributes = new Lazy<Collection<XmlAttribute>>();
        private readonly Lazy<Collection<Import>> _imports = new Lazy<Collection<Import>>();
        private readonly Lazy<Collection<Extension>> _extensions = new Lazy<Collection<Extension>>();
        private readonly Lazy<Collection<RootElement>> _rootElements = new Lazy<Collection<RootElement>>();
        private readonly Lazy<Collection<BpmnDiagram>> _bpmnDiagrams = new Lazy<Collection<BpmnDiagram>>();
        private readonly Lazy<Collection<Relationship>> _relationships = new Lazy<Collection<Relationship>>();

        [XmlAttribute("id", DataType = "ID")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("targetNamespace", DataType = "anyURI")]
        public string TargetNamespace { get; set; }

        [XmlAttribute("expressionLanguage", DataType = "anyURI")]
        [DefaultValue("http://www.w3.org/1999/XPath")]
        public string ExpressionLanguage { get; set; } = "http://www.w3.org/1999/XPath";

        [XmlAttribute("typeLanguage")]
        [DefaultValue("http://www.w3.org/2001/XMLSchema")]
        public string TypeLanguage { get; set; } = "http://www.w3.org/2001/XMLSchema";

        [XmlAttribute("exporter")]
        public string Exporter { get; set; }

        [XmlAttribute("exporterVersion")]
        public string ExporterVersion { get; set; }

        [XmlAnyAttribute]
        public Collection<XmlAttribute> ExtensionAttributes => _extensionAttributes.Value;

        [XmlElement("import")]
        public Collection<Import> Imports => _imports.Value;

        [XmlElement("extension")]
        public Collection<Extension> Extensions => _extensions.Value;

        
        [XmlElement("signal", Type = typeof(Signal))]
        [XmlElement("resource", Type = typeof(Resource))]
        [XmlElement("partnerRole", Type = typeof(PartnerRole))]
        [XmlElement("partnerEntity", Type = typeof(PartnerEntity))]
        [XmlElement("message", Type = typeof(Message))]
        [XmlElement("itemDefinition", Type = typeof(ItemDefinition))]
        [XmlElement("interface", Type = typeof(Interface))]
        [XmlElement("eventDefinition", Type = typeof(EventDefinition))]
        [XmlElement("timerEventDefinition", Type = typeof(TimerEventDefinition))]
        [XmlElement("terminateEventDefinition", Type = typeof(TerminateEventDefinition))]
        [XmlElement("signalEventDefinition", Type = typeof(SignalEventDefinition))]
        [XmlElement("messageEventDefinition", Type = typeof(MessageEventDefinition))]
        [XmlElement("linkEventDefinition", Type = typeof(LinkEventDefinition))]
        [XmlElement("escalationEventDefinition", Type = typeof(EscalationEventDefinition))]
        [XmlElement("errorEventDefinition", Type = typeof(ErrorEventDefinition))]
        [XmlElement("conditionalEventDefinition", Type = typeof(ConditionalEventDefinition))]
        [XmlElement("compensateEventDefinition", Type = typeof(CompensateEventDefinition))]
        [XmlElement("cancelEventDefinition", Type = typeof(CancelEventDefinition))]
        [XmlElement("escalation", Type = typeof(Escalation))]
        [XmlElement("error", Type = typeof(Error))]
        [XmlElement("endPoint", Type = typeof(EndPoint))]
        [XmlElement("dataStore", Type = typeof(DataStore))]
        [XmlElement("correlationProperty", Type = typeof(CorrelationProperty))]
        [XmlElement("collaboration", Type = typeof(Collaboration))]
        [XmlElement("globalConversation", Type = typeof(GlobalConversation))]
        [XmlElement("choreography", Type = typeof(Choreography))]
        [XmlElement("globalChoreographyTask", Type = typeof(GlobalChoreographyTask))]
        [XmlElement("category", Type = typeof(Category))]
        [XmlElement("callableElement", Type = typeof(CallableElement))]
        [XmlElement("process", Type = typeof(Process))]
        [XmlElement("globalTask", Type = typeof(GlobalTask))]
        [XmlElement("globalUserTask", Type = typeof(GlobalUserTask))]
        [XmlElement("globalScriptTask", Type = typeof(GlobalScriptTask))]
        [XmlElement("globalManualTask", Type = typeof(GlobalManualTask))]
        [XmlElement("globalBusinessRuleTask", Type = typeof(GlobalBusinessRuleTask))]
        public Collection<RootElement> RootElements => _rootElements.Value;

        [XmlElement("BPMNDiagram", Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
        public Collection<BpmnDiagram> BpmnDiagrams => _bpmnDiagrams.Value;

        [XmlElement("relationship")]
        public Collection<Relationship> Relationships => _relationships.Value;
    }
}
