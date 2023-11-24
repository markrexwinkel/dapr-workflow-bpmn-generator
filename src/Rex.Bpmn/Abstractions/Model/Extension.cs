using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tExtension", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("extension", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Extension
    {
        private readonly Lazy<Collection<Documentation>> _documentation = new Lazy<Collection<Documentation>>();

        [XmlElement("documentation")]
        public Collection<Documentation> Documentation => _documentation.Value;

        [XmlAttribute("definition")]
        public XmlQualifiedName Definition {get; set; }

        [XmlAttribute("mustUnderstand")]
        public bool MustUnderstand { get; set; }
    }
}