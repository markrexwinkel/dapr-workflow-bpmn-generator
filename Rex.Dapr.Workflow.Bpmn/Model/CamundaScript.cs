using System.Xml.Serialization;

namespace Rex.Dapr.Workflow.Bpmn.Model
{
    [XmlType(Namespace = "http://camunda.org/schema/1.0/bpmn")]
    [XmlRoot(Namespace = "http://camunda.org/schema/1.0/bpmn", IsNullable = false)]
    public class CamundaScript
    {
        [XmlAttribute("scriptFormat")]
        public string ScriptFormat { get; set; }

        [XmlAttribute("resource")]
        public string Resource { get; set; }

        [XmlText]
        public string InlineScript { get; set; }
    }
}
