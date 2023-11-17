using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tResourceParameterParameterBinding", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("resourceParameterBinding", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ResourceParameterBinding : BaseElement
    {
        [XmlElement("expression")]
        public Expression Expression { get; set; }

        [XmlAttribute("parameterRef")]
        public XmlQualifiedName ParameterRef { get; set; }
    }
}