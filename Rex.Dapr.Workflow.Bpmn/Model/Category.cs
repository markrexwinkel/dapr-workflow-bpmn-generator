using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tCategory", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("category", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Category : RootElement
    {
        private readonly Lazy<Collection<CategoryValue>> _values = new Lazy<Collection<CategoryValue>>();

        [XmlElement("categoryValue")]
        public Collection<CategoryValue> Values => _values.Value;

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}