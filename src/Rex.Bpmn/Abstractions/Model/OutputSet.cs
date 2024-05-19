using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tOutputSet", Namespace = Namespaces.Bpmn)]
[XmlRoot("outputSet", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class OutputSet : BaseElement
{
    private readonly Lazy<Collection<string>> _dataOutputRefs = new();
    private readonly Lazy<Collection<string>> _optionalOutputRefs = new();
    private readonly Lazy<Collection<string>> _whileExecutingOutputRefs = new();
    private readonly Lazy<Collection<string>> _inputSetRefs = new();

    [XmlElement("dataOutputRefs", DataType = "IDREF")]
    public Collection<string> DataOutputRefs => _dataOutputRefs.Value;

    [XmlElement("optionalOutputRefs", DataType = "IDREF")]
    public Collection<string> OptionalOutputRefs => _optionalOutputRefs.Value;

    [XmlElement("whileExecutingOutputRefs", DataType = "IDREF")]
    public Collection<string> WhileExecutingOutputRefs => _whileExecutingOutputRefs.Value;

    [XmlElement("inputSetRefs", DataType = "IDREF")]
    public Collection<string> InputSetRefs => _inputSetRefs.Value;

    [XmlAttribute("name")]
    public string Name { get; set; }
}