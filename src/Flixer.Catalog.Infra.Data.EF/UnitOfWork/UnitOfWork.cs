using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Infra.Data.EF.Context;

namespace Flixer.Catalog.Infra.Data.EF.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ILogger<UnitOfWork> _logger;
    private readonly FlixerCatalogDbContext _context;
    private readonly IDomainEventPublisher _publisher;

    public UnitOfWork(
        ILogger<UnitOfWork> logger, 
        FlixerCatalogDbContext context, 
        IDomainEventPublisher publisher
    )
    {
        _logger = logger;
        _context = context;
        _publisher = publisher;
    }

    public async Task<bool> Commit()
    {
        var aggregateRoots = _context.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(entry => entry.Entity.Events.Any())
            .Select(entry => entry.Entity).ToArray();

        _logger.LogInformation(
            "Commit: {AggregatesCount} aggregate roots with events.",
            aggregateRoots.Length);

        var events = aggregateRoots
            .SelectMany(aggregate => aggregate.Events).ToArray();

        _logger.LogInformation("Commit: {EventsCount} events raised.", events.Length);

        foreach (var @event in events)
            await _publisher.PublishAsync((dynamic)@event);

        foreach (var aggregate in aggregateRoots)
            aggregate.ClearEvents();

        return await _context.SaveChangesAsync() > 0;
    }
}