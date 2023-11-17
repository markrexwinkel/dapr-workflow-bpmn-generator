using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tErrorEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("errorEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ErrorEventDefinition : EventDefinition
    {
        [XmlAttribute("errorRef")]
        public XmlQualifiedName ErrorRef { get; set; }
    }
}