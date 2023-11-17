using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tChoreographyTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("choreographyTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class ChoreographyTask : ChoreographyActivity
    {
        private readonly Lazy<Collection<XmlQualifiedName>> _messageFlowRefs = new Lazy<Collection<XmlQualifiedName>>();

        [XmlElement("messageFlowRef")]
        public Collection<XmlQualifiedName> MessageFlowRefs => _messageFlowRefs.Value;
    }
}