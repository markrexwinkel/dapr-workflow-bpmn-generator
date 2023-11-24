using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tScriptTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("scriptTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ScriptTask : Task
    {
        [XmlElement("script")]
        public XmlNode Script { get; set; }

        [XmlAttribute("scriptFormat")]
        public string ScriptFormat { get; set; }
    }
}