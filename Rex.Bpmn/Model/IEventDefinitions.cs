using System.Collections.ObjectModel;
using System.Xml;

namespace Rex.Bpmn.Model
{
    public interface IEventDefinitions
    {
        Collection<EventDefinition> EventDefinitions { get; }

        Collection<XmlQualifiedName> EventDefinitionRefs { get; }
    }
}