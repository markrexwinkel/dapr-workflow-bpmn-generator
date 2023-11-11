using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tCallActivity", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("callActivity", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class CallActivity : Activity
    {
        [XmlAttribute("calledElement")]
        public XmlQualifiedName CalledElement { get; set; }
    }
}