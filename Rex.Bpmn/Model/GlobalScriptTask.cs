using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tGlobalScriptTask", Namespace="http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("globalScriptTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class GlobalScriptTask : GlobalTask
    {
        [XmlElement("script")]
        public XmlNode Script { get; set; }

        [XmlAttribute("scriptLanguage", DataType = "anyURI")]
        public string ScriptLanguage { get; set; }
    }
}