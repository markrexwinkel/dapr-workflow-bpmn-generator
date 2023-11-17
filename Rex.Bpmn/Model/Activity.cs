using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model;

[XmlInclude(typeof(Task))]
[XmlInclude(typeof(UserTask))]
[XmlInclude(typeof(ServiceTask))]
[XmlInclude(typeof(SendTask))]
[XmlInclude(typeof(ScriptTask))]
[XmlInclude(typeof(ReceiveTask))]
[XmlInclude(typeof(ManualTask))]
[XmlInclude(typeof(BusinessRuleTask))]
[XmlInclude(typeof(SubProcess))]
[XmlInclude(typeof(Transaction))]
[XmlInclude(typeof(AdHocSubProcess))]
[XmlInclude(typeof(CallActivity))]
[XmlType("tActivity", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
[XmlRoot("activity", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
public class Activity : FlowNode, IDefaultSequence
{
    private readonly Lazy<Collection<Property>> _properties = new();
    private readonly Lazy<Collection<DataInputAssociation>> _dataInputAssociations = new();
    private readonly Lazy<Collection<DataOutputAssociation>> _dataOutputAssociations = new();
    private readonly Lazy<Collection<ResourceRole>> _resourceRoles = new();

    [XmlElement("ioSpecification")]
    public InputOutputSpecification IoSpecification { get; set; }

    [XmlElement("property")]
    public Collection<Property> Properties => _properties.Value;

    [XmlElement("dataInputAssociation")]
    public Collection<DataInputAssociation> DataInputAssociations => _dataInputAssociations.Value;

    [XmlElement("dataOutputAssociation")]
    public Collection<DataOutputAssociation> DataOutputAssociations => _dataOutputAssociations.Value;

    [XmlElement("resourceRole")]
    public Collection<ResourceRole> ResourceRoles => _resourceRoles.Value;

    [XmlElement("loopCharacteristics")]
    public LoopCharacteristics LoopCharacteristics { get; set; }

    [XmlAttribute("isForComposation")]
    public bool IsForComposation { get; set; }

    [XmlAttribute("startQuantity")]
    [DefaultValue(1)]
    public int StartQuantity { get; set; } = 1;

    [XmlAttribute("completionQuantity")]
    [DefaultValue(1)]
    public int CompletionQuantity { get; set; } = 1;

    [XmlAttribute("default", DataType = "IDREF")]
    public string Default { get; set; }
}