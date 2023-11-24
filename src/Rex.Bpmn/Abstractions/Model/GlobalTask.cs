using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(GlobalUserTask))]
    [XmlInclude(typeof(GlobalScriptTask))]
    [XmlInclude(typeof(GlobalManualTask))]
    [XmlInclude(typeof(GlobalBusinessRuleTask))]
    [XmlType("tGlobalTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("globalTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class GlobalTask : CallableElement
    {
        private readonly Lazy<Collection<ResourceRole>> _resourceRoles = new Lazy<Collection<ResourceRole>>();

        [XmlElement("resourceRole")]
        public Collection<ResourceRole> ResourceRoles => _resourceRoles.Value;
    }
}