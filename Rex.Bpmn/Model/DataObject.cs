using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tDataObject", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("dataObject", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class DataObject : FlowElement
    {
        [XmlElement("dataState")]
        public DataState DataState { get; set; }

        [XmlAttribute("itemSubjectRef")]
        public XmlQualifiedName ItemSubjectRef { get; set; }

        [XmlAttribute("isCollection")]
        public bool IsCollection { get; set; }
    }
}