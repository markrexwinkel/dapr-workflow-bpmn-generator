using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.DaprBpmn)]
[XmlRoot("inputOutput", Namespace = Namespaces.DaprBpmn, IsNullable = false)]
public class DaprInputOutput
{
    private readonly Lazy<Collection<DaprParameter>> _inputParameters = new();
    private readonly Lazy<Collection<DaprParameter>> _outputParameters = new();

    [XmlElement("inputParameter")]
    public Collection<DaprParameter> InputParameters => _inputParameters.Value;

    [XmlElement("outputParameter")]
    public Collection<DaprParameter> OutputParameters => _outputParameters.Value;
}
