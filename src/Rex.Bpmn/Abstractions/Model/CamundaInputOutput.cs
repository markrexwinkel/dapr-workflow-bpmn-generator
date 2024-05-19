using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.CamundaBpmn)]
[XmlRoot("inputOutput", Namespace = Namespaces.CamundaBpmn, IsNullable = false)]
public class CamundaInputOutput
{
    private readonly Lazy<Collection<CamundaInputParameter>> _inputParameters = new();
    private readonly Lazy<Collection<CamundaOutputParameter>> _outputParameters = new();

    [XmlElement("inputParameter")]
    public Collection<CamundaInputParameter> InputParameters => _inputParameters.Value;

    [XmlElement("outputParameter")]
    public Collection<CamundaOutputParameter> OutputParameters => _outputParameters.Value;
}
