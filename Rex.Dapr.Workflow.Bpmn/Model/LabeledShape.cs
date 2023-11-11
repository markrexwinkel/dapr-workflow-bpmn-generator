using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlInclude(typeof(BpmnShape))]
    [XmlType(Namespace = "http://www.omg.org/spec/DD/20100524/DI")]
    [XmlRoot(Namespace = "http://www.omg.org/spec/DD/20100524/DI", IsNullable = false)]
    public abstract class LabeledShape : Shape
    {
    }
}