using System.Xml.Serialization;

namespace Rex.Dapr.Workflow.Bpmn.Model
{
    [XmlType(Namespace = "http://camunda.org/schema/1.0/bpmn")]
    [XmlRoot(Namespace = "http://camunda.org/schema/1.0/bpmn", IsNullable = false)]
    public class CamundaValue
    {
        [XmlText]
        public string Value { get; set; }
    }
}
