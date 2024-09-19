using Flixer.Catalog.Domain.SeedWork;

namespace Flixer.Catalog.Domain.Contracts;

public interface IDomainEventPublisher
{
    Task PublishAsync<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent;
}