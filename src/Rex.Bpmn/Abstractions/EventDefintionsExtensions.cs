using Rex.Bpmn.Abstractions.Model;
using System.Collections.Generic;
using System.Linq;

namespace Rex.Bpmn.Abstractions
{
    public static class EventDefintionsExtensions
    {
        public static EventDefinition GetEventDefinition(this IEventDefinitions elem, Definitions definitions = null)
        {
            var q = (IEnumerable<EventDefinition>) elem.EventDefinitions;
            if (definitions is not null)
            {
                q = q.Concat(elem.EventDefinitionRefs.SelectMany(x => definitions.RootElements.OfType<EventDefinition>().Where(y => y.Id == x.ToString())));
            }
            return q.FirstOrDefault();
        }
    }
}
