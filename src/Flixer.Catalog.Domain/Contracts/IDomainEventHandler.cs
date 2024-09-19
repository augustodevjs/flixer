using Flixer.Catalog.Domain.SeedWork;

namespace Flixer.Catalog.Domain.Contracts;

public interface IDomainEventHandler<TDomainEvent> where TDomainEvent: DomainEvent
{
    Task HandleAsync(TDomainEvent domainEvent);
}