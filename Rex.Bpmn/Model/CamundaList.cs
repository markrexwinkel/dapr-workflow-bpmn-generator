using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Dapr.Workflow.Bpmn.Model
{
    [XmlType(Namespace = "http://camunda.org/schema/1.0/bpmn")]
    [XmlRoot(Namespace = "http://camunda.org/schema/1.0/bpmn", IsNullable = false)]
    public class CamundaList
    {
        private readonly Lazy<Collection<CamundaValue>> _values = new Lazy<Collection<CamundaValue>>();

        [XmlElement("value")]
        public Collection<CamundaValue> Values => _values.Value;
    }
}
