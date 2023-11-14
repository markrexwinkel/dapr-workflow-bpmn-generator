using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Dapr.Workflow.Bpmn.Model
{
    [XmlType(Namespace = "http://camunda.org/schema/1.0/bpmn")]
    [XmlRoot(Namespace = "http://camunda.org/schema/1.0/bpmn", IsNullable = false)]
    public class CamundaMap
    {
        private readonly Lazy<Collection<CamundaEntry>> _entries = new Lazy<Collection<CamundaEntry>>();

        [XmlElement("entry")]
        public Collection<CamundaEntry> Entries => _entries.Value;
    }
}
