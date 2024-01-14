using Flixer.Catalog.Infra.Data.EF.Context;
using Flixer.Catalog.Application.Contracts.UnityOfWork;

namespace Flixer.Catalog.Infra.Data.EF;

public class UnityOfWork : IUnityOfWork
{
    private readonly FlixerCatalogDbContext _context;

    public UnityOfWork(FlixerCatalogDbContext context)
        => _context = context;

    public Task Commit(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);

    public Task Rollback(CancellationToken cancellationToken)
        => Task.CompletedTask;
}