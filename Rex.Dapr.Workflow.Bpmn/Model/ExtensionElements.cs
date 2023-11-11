using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType(Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("extensionElements", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ExtensionElements
    {
        private readonly Lazy<Collection<XmlElement>> _elements = new Lazy<Collection<XmlElement>>();

        [XmlAnyElement]
        public Collection<XmlElement> Elements  => _elements.Value;
    }
}