using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tInputOutputSpecification", Namespace = Namespaces.Bpmn)]
[XmlRoot("ioSpecification", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class InputOutputSpecification : BaseElement
{
    private readonly Lazy<Collection<DataInput>> _dataInputs = new();
    private readonly Lazy<Collection<DataOutput>> _dataOutputs = new();
    private readonly Lazy<Collection<InputSet>> _inputSets = new();
    private readonly Lazy<Collection<OutputSet>> _outputSets = new();

    [XmlElement("dataInput")]
    public Collection<DataInput> DataInputs => _dataInputs.Value;

    [XmlElement("dataOutput")]
    public Collection<DataOutput> DataOutputs => _dataOutputs.Value;

    [XmlElement("inputSet")]
    public Collection<InputSet> InputSets => _inputSets.Value;

    [XmlElement("outputSet")]
    public Collection<OutputSet> OutputSets => _outputSets.Value;
}