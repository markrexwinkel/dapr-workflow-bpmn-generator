using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tCompensateEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("compensateEventDefinition", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class CompensateEventDefinition : EventDefinition
    {
        [XmlAttribute("waitForCompletion")]
        public bool WaitForCompletion { get; set; }

        [XmlAttribute("activityRef")]
        public XmlQualifiedName ActivityRef { get; set; }
    }
}