using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tFormalExpression", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("formalExpression", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class FormalExpression : Expression
    {
        [XmlAttribute("language", DataType = "anyURI")]
        public string Language { get; set; }

        [XmlAttribute("evaluatesToTypeRef")]
        public XmlQualifiedName EvaluatesToTypeRef { get; set; }
    }
}