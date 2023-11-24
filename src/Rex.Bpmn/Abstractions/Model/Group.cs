using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tGroup", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("group", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Group : Artifact
    {
        public XmlQualifiedName CategoryValueRef { get; set; }
    }
}