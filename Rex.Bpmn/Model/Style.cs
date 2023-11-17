using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlInclude(typeof(BpmnLabelStyle))]
    [XmlType(Namespace = "http://www.omg.org/spec/DD/20100524/DI")]
    [XmlRoot(Namespace = "http://www.omg.org/spec/DD/20100524/DI", IsNullable = false)]
    public abstract class Style
    {
        [XmlAttribute("id", DataType = "ID")]
        public string Id { get; set; }
    }
}