namespace Flixer.Catalog.Domain.SeedWork;

public abstract class Entity
{
    public Guid Id { get; } = Guid.NewGuid();
}
