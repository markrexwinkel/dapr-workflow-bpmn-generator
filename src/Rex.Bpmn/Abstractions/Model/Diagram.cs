using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(BpmnDiagram))]
    [XmlType(Namespace = "http://www.omg.org/spec/DD/20100524/DI")]
    [XmlRoot(Namespace = "http://www.omg.org/spec/DD/20100524/DI", IsNullable = false)]
    public abstract class Diagram
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("documentation")]
        public string Documentation { get; set; }

        [XmlAttribute("resolution")]
        public double Resolution { get; set; }

        [XmlIgnore]
        public bool ResolutionSpecified { get; set; }

        [XmlAttribute("id", DataType = "ID")]
        public string Id { get; set; }
    }
}