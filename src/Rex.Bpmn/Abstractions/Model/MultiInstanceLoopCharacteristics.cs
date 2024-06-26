﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tMultiInstanceLoopCharacteristics", Namespace = Namespaces.Bpmn)]
[XmlRoot("multiInstanceLoopCharacteristics", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class MultiInstanceLoopCharacteristics : LoopCharacteristics
{
    private readonly Lazy<Collection<ComplexBehaviorDefinition>> _complexBehaviorDefinitions = new();

    [XmlElement("loopCardinality")]
    public Expression LoopCardinality { get; set; }

    [XmlElement("loopDataInputRef")]
    public XmlQualifiedName LoopDataInputRef { get; set; }

    [XmlElement("loopDataOutputRef")]
    public XmlQualifiedName LoopDataOutputRef { get; set; }

    [XmlElement("dataInputItem")]
    public DataInput InputDataItem { get; set; }

    [XmlElement("outputDataItem")]
    public DataOutput OutputDataItem { get; set; }

    [XmlElement("complexBehaviorDefinition")]
    public Collection<ComplexBehaviorDefinition> ComplexBehaviorDefinitions => _complexBehaviorDefinitions.Value;

    [XmlElement("completionCondition")]
    public Expression CompletionCondition { get; set; }

    [XmlAttribute("isSequential")]
    public bool IsSequential { get; set; }

    [XmlAttribute("behavior")]
    [DefaultValue(MultiInstanceFlowCondition.All)]
    public MultiInstanceFlowCondition Behavior { get; set; } = MultiInstanceFlowCondition.All;

    [XmlAttribute("oneBehaviorEventRef")]
    public XmlQualifiedName OneBehaviorEventRef { get; set; }

    [XmlAttribute("noneBehaviorEventRef")]
    public XmlQualifiedName NoneBehaviorEventRef { get; set; }
}