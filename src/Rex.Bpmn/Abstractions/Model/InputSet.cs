using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tInputSet", Namespace = Namespaces.Bpmn)]
[XmlRoot("inputSet", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class InputSet : BaseElement
{
    private readonly Lazy<Collection<string>> _dataInputRefs = new();
    private readonly Lazy<Collection<string>> _optionalInputRefs = new();
    private readonly Lazy<Collection<string>> _whileExecutingInputRefs = new();
    private readonly Lazy<Collection<string>> _outputSetRefs = new();

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