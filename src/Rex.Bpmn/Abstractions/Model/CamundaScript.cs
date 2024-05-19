using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.CamundaBpmn)]
[XmlRoot(Namespace = Namespaces.CamundaBpmn, IsNullable = false)]
public class CamundaScript
{
    [XmlAttribute("scriptFormat")]
    public string ScriptFormat { get; set; }

    [XmlAttribute("resource")]
    public string Resource { get; set; }

    [XmlText]
    public string InlineScript { get; set; }
}
