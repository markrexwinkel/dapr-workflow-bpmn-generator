using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(Performer))]
    [XmlType("tResourceRole", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("resourceRole", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ResourceRole : BaseElement
    {
        private readonly Lazy<Collection<object>> _items = new Lazy<Collection<object>>();

        [XmlElement("resourceAssignmentExpression", typeof(ResourceAssignmentExpression))]
        [XmlElement("resourceParameterBinding", typeof(ResourceParameterBinding))]
        [XmlElement("resourceRef", typeof(XmlQualifiedName))]
        public Collection<object> Items => _items.Value;

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}