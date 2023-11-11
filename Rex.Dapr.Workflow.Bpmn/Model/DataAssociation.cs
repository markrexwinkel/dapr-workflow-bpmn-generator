using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlInclude(typeof(DataOutputAssociation))]
    [XmlInclude(typeof(DataInputAssociation))]
    [XmlType("tDataAssociation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("dataAssociation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class DataAssociation : BaseElement
    {
        private readonly Lazy<Collection<string>> _sourceRefs = new Lazy<Collection<string>>();
        private readonly Lazy<Collection<Assignment>> _assignments = new Lazy<Collection<Assignment>>();

        [XmlElement("sourceRef", DataType = "IDREF")]
        public Collection<string> SourceRefs => _sourceRefs.Value;

        [XmlElement("targetRef", DataType = "IDREF")]
        public string TargetRef { get; set; }

        [XmlElement("transformation")]
        public FormalExpression Transformation { get; set; }

        [XmlElement("assignment")]
        public Collection<Assignment> Assignments => _assignments.Value;
    }
}