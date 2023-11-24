using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlInclude(typeof(Edge))]
    [XmlInclude(typeof(LabeledEdge))]
    [XmlInclude(typeof(BpmnEdge))]
    [XmlInclude(typeof(Node))]
    [XmlInclude(typeof(Plane))]
    [XmlInclude(typeof(BpmnPlane))]
    [XmlInclude(typeof(Label))]
    [XmlInclude(typeof(BpmnLabel))]
    [XmlInclude(typeof(Shape))]
    [XmlInclude(typeof(LabeledShape))]
    [XmlInclude(typeof(BpmnShape))]
    [XmlType(Namespace = "http://www.omg.org/spec/DD/20100524/DI")]
    [XmlRoot(Namespace = "http://www.omg.org/spec/DD/20100524/DI", IsNullable = false)]
    public abstract class DiagramElement
    {
        private readonly Lazy<Collection<XmlAttribute>> _extensionAttributes = new Lazy<Collection<XmlAttribute>>();

        [XmlElement("extension")]
        public DiagramElementExtension Extension { get; set; }

        [XmlAttribute("id", DataType = "ID")]
        public string Id { get; set; }

        [XmlAnyAttribute]
        public Collection<XmlAttribute> ExtensionAttributes => _extensionAttributes.Value;
    }
}