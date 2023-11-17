using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("BPMNDiagram", Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
    [XmlRoot("BPMNDiagram", Namespace = "http://www.omg.org/spec/BPMN/20100524/DI", IsNullable = false)]
    public class BpmnDiagram : Diagram
    {
        private readonly Lazy<Collection<BpmnLabelStyle>> _labelStyles = new Lazy<Collection<BpmnLabelStyle>>();

        [XmlElement("BPMNPlane")]
        public BpmnPlane Plane { get; set;}

        [XmlElement("BPMNLabelStyle")]
        public Collection<BpmnLabelStyle> LabelStyles => _labelStyles.Value;
    }
}