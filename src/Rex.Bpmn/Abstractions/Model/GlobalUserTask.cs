using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tGlobalUserTask", Namespace = Namespaces.Bpmn)]
[XmlRoot("globalUserTask", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class GlobalUserTask : GlobalTask
{
    private readonly Lazy<Collection<Rendering>> _renderings = new();

    [XmlElement("rendering")]
    public Collection<Rendering> Renderings => _renderings.Value;

    [XmlAttribute("implementation")]
    [DefaultValue("##unspecified")]
    public string Implementation { get; set; } = "##unspecified";
}