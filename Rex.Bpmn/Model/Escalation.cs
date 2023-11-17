using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tEscalation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("escalation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Escalation : RootElement
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("escalationCode")]
        public string EscalationCode { get; set; }

        [XmlAttribute("structureRef")]
        public XmlQualifiedName StructureRef { get; set; }
    }
}