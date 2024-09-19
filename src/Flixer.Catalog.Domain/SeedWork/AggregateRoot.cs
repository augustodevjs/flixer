using System.Collections.ObjectModel;

namespace Flixer.Catalog.Domain.SeedWork;

public abstract class AggregateRoot : Entity
{
    private readonly List<DomainEvent> _events = new();
    
    public IReadOnlyCollection<DomainEvent> Events 
        => new ReadOnlyCollection<DomainEvent>(_events);
    
    public void RaiseEvent(DomainEvent @event) => _events.Add(@event);
    public void ClearEvents() => _events.Clear();
}
