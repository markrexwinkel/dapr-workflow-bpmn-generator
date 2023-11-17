using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tSignal", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("signal", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Signal : RootElement
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("structureRef")]
        public XmlQualifiedName StructureRef { get; set; }
    }
}