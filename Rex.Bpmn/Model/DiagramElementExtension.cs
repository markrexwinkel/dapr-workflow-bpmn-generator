using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType(AnonymousType = true, Namespace = "http://www.omg.org/spec/DD/20100524/DI")]
    public class DiagramElementExtension
    {
        private readonly Lazy<Collection<XmlElement>> _elements = new Lazy<Collection<XmlElement>>();

        [XmlAnyElement]
        public Collection<XmlElement> Elements => _elements.Value;
    }
}