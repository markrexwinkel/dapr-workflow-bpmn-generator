using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tAssociation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("association", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Association : Artifact
    {
        public Association()
        {
            AssociationDirection = AssociationDirection.None;
        }

        [XmlAttribute("sourceRef")]
        public XmlQualifiedName SourceRef { get; set; }
        [XmlAttribute("targetRef")]
        public XmlQualifiedName TargetRef { get; set; }
        [XmlAttribute("associationDirection")]
        [DefaultValue(AssociationDirection.None)]
        public AssociationDirection AssociationDirection { get; set; } = AssociationDirection.None;
    }
}
