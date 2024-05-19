using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.CamundaBpmn)]
[XmlRoot(Namespace = Namespaces.CamundaBpmn, IsNullable = false)]
public class CamundaList
{
    private readonly Lazy<Collection<CamundaValue>> _values = new();

    [XmlElement("value")]
    public Collection<CamundaValue> Values => _values.Value;
}
