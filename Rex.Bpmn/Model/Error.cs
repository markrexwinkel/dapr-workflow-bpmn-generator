using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tError", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("error", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Error : RootElement
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("errorCode")]
        public string ErrorCode { get; set; }

        [XmlAttribute("structureRef")]
        public XmlQualifiedName StructureRef { get; set; }
    }
}