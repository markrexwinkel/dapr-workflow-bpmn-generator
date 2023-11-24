using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model;

[XmlType(Namespace = Namespaces.CamundaBpmn)]
[XmlRoot(Namespace = Namespaces.CamundaBpmn, IsNullable = false)]
public class CamundaMap
{
    private readonly Lazy<Collection<CamundaEntry>> _entries = new();

    [XmlElement("entry")]
    public Collection<CamundaEntry> Entries => _entries.Value;
}
