using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tProperty", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("property", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Property : BaseElement
    {
        [XmlElement("dataState")]
        public DataState DataState { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("itemSubjectRef")]
        public XmlQualifiedName ItemSubjectRef { get; set; }
    }
}