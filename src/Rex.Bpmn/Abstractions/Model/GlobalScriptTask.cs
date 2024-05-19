using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tGlobalScriptTask", Namespace=Namespaces.Bpmn)]
[XmlRoot("globalScriptTask", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class GlobalScriptTask : GlobalTask
{
    [XmlElement("script")]
    public XmlNode Script { get; set; }

    [XmlAttribute("scriptLanguage", DataType = "anyURI")]
    public string ScriptLanguage { get; set; }
}