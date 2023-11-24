using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType(Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("import", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Import
    {
        [XmlAttribute("namespace", DataType = "anyURI")]
        public string Namespace { get; set; }

        [XmlAttribute("location")]
        public string Location { get; set; }

        [XmlAttribute("importType", DataType = "anyURI")]
        public string ImportType { get; set; }
    }
}