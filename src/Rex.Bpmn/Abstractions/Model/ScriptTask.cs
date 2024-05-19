using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tScriptTask", Namespace = Namespaces.Bpmn)]
[XmlRoot("scriptTask", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class ScriptTask : Task
{
    [XmlElement("script")]
    public XmlNode Script { get; set; }

    [XmlAttribute("scriptFormat")]
    public string ScriptFormat { get; set; }
}