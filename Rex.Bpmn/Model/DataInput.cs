using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tDataInput", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("dataInput", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class DataInput : BaseElement
    {
        [XmlElement("dataState")]
        public DataState DataState { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("itemSubjectRef")]
        public XmlQualifiedName ItemSubjectRef { get; set; }

        [XmlAttribute("isCollection")]
        public bool IsCollection { get; set; }
    }
}