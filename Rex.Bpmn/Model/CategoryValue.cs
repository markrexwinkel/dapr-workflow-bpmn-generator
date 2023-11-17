using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tCategoryValue", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("categoryValue", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class CategoryValue : BaseElement
    {
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}