using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlInclude(typeof(GlobalUserTask))]
[XmlInclude(typeof(GlobalScriptTask))]
[XmlInclude(typeof(GlobalManualTask))]
[XmlInclude(typeof(GlobalBusinessRuleTask))]
[XmlType("tGlobalTask", Namespace = Namespaces.Bpmn)]
[XmlRoot("globalTask", Namespace = Namespaces.Bpmn, IsNullable = false)]
public class GlobalTask : CallableElement
{
    private readonly Lazy<Collection<ResourceRole>> _resourceRoles = new();

    [XmlElement("resourceRole")]
    public Collection<ResourceRole> ResourceRoles => _resourceRoles.Value;
}