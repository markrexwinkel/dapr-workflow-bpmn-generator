using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType("tAssociation", Namespace = Namespaces.Bpmn)]
[XmlRoot("association", Namespace = Namespaces.Bpmn, IsNullable = false)]
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
