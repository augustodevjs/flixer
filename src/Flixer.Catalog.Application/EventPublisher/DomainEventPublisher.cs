using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Flixer.Catalog.Application.EventPublisher;

public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent
    {
        var handlers = _serviceProvider
            .GetServices<IDomainEventHandler<TDomainEvent>>();

        var domainEventHandlers = handlers.ToList();
        
        if (!domainEventHandlers.Any()) return;
        
        foreach (var handler in domainEventHandlers)
            await handler.HandleAsync(domainEvent);
    }
}