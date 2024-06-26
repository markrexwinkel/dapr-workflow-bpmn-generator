﻿using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tDataStore", Namespace = Namespaces.Bpmn)]
[XmlRoot("dataStore", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class DataStore : RootElement
{
    [XmlElement("dataState")]
    public DataState DataState { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("capacity")]
    public int Capacity { get; set; } = -1;

    [XmlAttribute("isUnlimited")]
    [DefaultValue(true)]
    public bool IsUnlimited { get; set; } = true;

    [XmlAttribute("itemSubjectRef")]
    public XmlQualifiedName ItemSubjectRef { get; set; }
}