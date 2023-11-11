using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tInterface", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("interface", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Interface : RootElement
    {
        private Lazy<Collection<Operation>> _operations = new Lazy<Collection<Operation>>();

        [XmlElement("operation")]
        public Collection<Operation> Operations => _operations.Value;

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("implementationRef")]
        public XmlQualifiedName ImplementationRef { get; set; }
    }
}