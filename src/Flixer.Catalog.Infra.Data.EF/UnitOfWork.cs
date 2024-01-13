using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Infra.Data.EF.Context;

namespace Flixer.Catalog.Infra.Data.EF;

public class UnitOfWork : IUnityOfWork
{
    private readonly FlixerCatalogDbContext _context;

    public UnitOfWork(FlixerCatalogDbContext context)
        => _context = context;

    public Task Commit(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task RollBack(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
