using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Dapr.Workflow.Bpmn.Model
{
    [XmlType(Namespace = "http://camunda.org/schema/1.0/bpmn")]
    [XmlRoot("inputOutput", Namespace = "http://camunda.org/schema/1.0/bpmn", IsNullable = false)]
    public class CamundaInputOutput
    {
        private readonly Lazy<Collection<CamundaInputParameter>> _inputParameters = new Lazy<Collection<CamundaInputParameter>>();
        private readonly Lazy<Collection<CamundaOutputParameter>> _outputParameters = new Lazy<Collection<CamundaOutputParameter>>();

        [XmlElement("inputParameter")]
        public Collection<CamundaInputParameter> InputParameters => _inputParameters.Value;

        [XmlElement("outputParameter")]
        public Collection<CamundaOutputParameter> OutputParameters => _outputParameters.Value;
    }
}
