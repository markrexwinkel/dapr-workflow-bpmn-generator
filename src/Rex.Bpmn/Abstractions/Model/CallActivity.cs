using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tCallActivity", Namespace = Namespaces.Bpmn)]
[XmlRoot("callActivity", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class CallActivity : Activity
{
    [XmlAttribute("calledElement")]
    public XmlQualifiedName CalledElement { get; set; }
}