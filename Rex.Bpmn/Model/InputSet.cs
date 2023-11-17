using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tInputSet", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("inputSet", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class InputSet : BaseElement
    {
        private readonly Lazy<Collection<string>> _dataInputRefs = new Lazy<Collection<string>>();
        private readonly Lazy<Collection<string>> _optionalInputRefs = new Lazy<Collection<string>>();
        private readonly Lazy<Collection<string>> _whileExecutingInputRefs = new Lazy<Collection<string>>();
        private readonly Lazy<Collection<string>> _outputSetRefs = new Lazy<Collection<string>>();

        [XmlElement("dataInputRefs", DataType = "IDREF")]
        public Collection<string> DataInputRefs => _dataInputRefs.Value;

        [XmlElement("optionalInputRefs", DataType = "IDREF")]
        public Collection<string> OptionalInputRefs => _optionalInputRefs.Value;

        [XmlElement("whileExecutingInputRefs", DataType = "IDREF")]
        public Collection<string> WhileExecutingInputRefs => _whileExecutingInputRefs.Value;

        [XmlElement("outputSetRefs", DataType = "IDREF")]
        public Collection<string> OutputSetRefs => _outputSetRefs.Value;

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}