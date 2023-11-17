using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tAdHocSubProcess", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("adHocSubProcess", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class AdHocSubProcess : SubProcess
    {
        [XmlElement("completionCondition")]
        public Expression CompletionCondition { get; set; }

        [XmlAttribute("cancelRemainingInstances")]
        [DefaultValue(true)]
        public bool CancelRemainingInstances { get; set; } = true;

        [XmlAttribute("ordering")]
        public AdHocOrdering Ordering { get; set; }
    }


}