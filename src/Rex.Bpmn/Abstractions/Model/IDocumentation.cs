using System.Collections.ObjectModel;

namespace Rex.Bpmn.Abstractions.Model
{
    public interface IDocumentation
    {
        Collection<Documentation> Documentation { get; }
    }
}
