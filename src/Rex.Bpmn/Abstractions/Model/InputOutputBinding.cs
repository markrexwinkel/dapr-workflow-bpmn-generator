using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tInputOutputBinding", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("ioBinding", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class InputOutputBinding : BaseElement
    {
        [XmlAttribute("operationRef")]
        public XmlQualifiedName OperationRef { get; set; }

        [XmlAttribute("inputDataRef", DataType = "IDREF")]
        public string InputDataRef { get; set; }

        [XmlAttribute("outputDataRef", DataType = "IDREF")]
        public string OutputDataRef { get; set; }
    }
}