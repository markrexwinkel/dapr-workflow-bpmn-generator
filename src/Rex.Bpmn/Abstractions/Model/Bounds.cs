using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType(Namespace = "http://www.omg.org/spec/DD/20100524/DC")]
    [XmlRoot(Namespace = "http://www.omg.org/spec/DD/20100524/DC", IsNullable = false)]
    public class Bounds
    {
        [XmlAttribute("x")]
        public double X { get; set; }

        [XmlAttribute("y")]
        public double Y { get; set; }

        [XmlAttribute("width")]
        public double Width { get; set; }

        [XmlAttribute("height")]
        public double Height { get; set; }
    }
}