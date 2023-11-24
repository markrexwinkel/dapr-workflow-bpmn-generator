using System.Collections.ObjectModel;
using System.Xml;

namespace Rex.Bpmn.Abstractions.Model
{
    public interface IExtensible : IIdentifiable
    {
        Collection<XmlAttribute> ExtensionAttributes { get; }
        ExtensionElements ExtensionElements { get; }
    }
}
