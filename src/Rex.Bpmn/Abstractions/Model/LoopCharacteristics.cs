using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(StandardLoopCharacteristics))]
[XmlInclude(typeof(MultiInstanceLoopCharacteristics))]
[XmlType("tLoopCharacteristics", Namespace = Namespaces.Bpmn)]
[XmlRoot("loopCharacteristics", Namespace = Namespaces.Bpmn, IsNullable = false)]
public abstract class LoopCharacteristics : BaseElement
{
}