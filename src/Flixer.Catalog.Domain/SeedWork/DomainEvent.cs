namespace Flixer.Catalog.Domain.SeedWork;

public abstract class DomainEvent
{
    public DateTime OccurredOn { get; set; } = DateTime.Now;
}