using System.Xml.Serialization;

namespace Rex.Dapr.Workflow.Bpmn.Model
{
    [XmlType(Namespace = "http://camunda.org/schema/1.0/bpmn")]
    [XmlRoot(Namespace = "http://camunda.org/schema/1.0/bpmn", IsNullable = false)]
    public class CamundaOutputParameter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public string Value { get; set; }

        [XmlElement("map")]
        public CamundaMap Map { get; set; }

        [XmlElement("list")]
        public CamundaList List { get; set; }

        [XmlElement("script")]
        public CamundaScript Script { get; set; }
    }
}
