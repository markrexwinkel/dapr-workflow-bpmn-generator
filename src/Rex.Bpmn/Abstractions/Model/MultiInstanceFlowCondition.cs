using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tMultiInstanceFlowCondition", Namespace = Namespaces.Bpmn)]
public enum MultiInstanceFlowCondition
{
    None,
    One,
    All,
    Complex
}