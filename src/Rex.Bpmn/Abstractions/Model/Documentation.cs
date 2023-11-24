using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType(Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("documentation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Documentation
    {
        private readonly Lazy<Collection<XmlElement>> _extensionElements = new Lazy<Collection<XmlElement>>();

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("textFormat")]
        [DefaultValue("text/plain")]
        public string TextFormat { get; set; } = "text/plain";

        [XmlAnyElement]
        public Collection<XmlElement> ExtensionElements => _extensionElements.Value;
    }
}