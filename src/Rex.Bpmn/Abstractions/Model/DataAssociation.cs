using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(DataOutputAssociation))]
[XmlInclude(typeof(DataInputAssociation))]
[XmlType("tDataAssociation", Namespace = Namespaces.Bpmn)]
[XmlRoot("dataAssociation", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class DataAssociation : BaseElement
{
    private readonly Lazy<Collection<string>> _sourceRefs = new();
    private readonly Lazy<Collection<Assignment>> _assignments = new();

    [XmlElement("sourceRef", DataType = "IDREF")]
    public Collection<string> SourceRefs => _sourceRefs.Value;

    [XmlElement("targetRef", DataType = "IDREF")]
    public string TargetRef { get; set; }

    [XmlElement("transformation")]
    public FormalExpression Transformation { get; set; }

    [XmlElement("assignment")]
    public Collection<Assignment> Assignments => _assignments.Value;
}