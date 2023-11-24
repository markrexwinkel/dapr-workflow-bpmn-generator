using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tInputOutputSpecification", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("ioSpecification", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class InputOutputSpecification : BaseElement
    {
        private readonly Lazy<Collection<DataInput>> _dataInputs = new Lazy<Collection<DataInput>>();
        private readonly Lazy<Collection<DataOutput>> _dataOutputs = new Lazy<Collection<DataOutput>>();
        private readonly Lazy<Collection<InputSet>> _inputSets = new Lazy<Collection<InputSet>>();
        private readonly Lazy<Collection<OutputSet>> _outputSets = new Lazy<Collection<OutputSet>>();

        [XmlElement("dataInput")]
        public Collection<DataInput> DataInputs => _dataInputs.Value;

        [XmlElement("dataOutput")]
        public Collection<DataOutput> DataOutputs => _dataOutputs.Value;

        [XmlElement("inputSet")]
        public Collection<InputSet> InputSets => _inputSets.Value;

        [XmlElement("outputSet")]
        public Collection<OutputSet> OutputSets => _outputSets.Value;
    }
}