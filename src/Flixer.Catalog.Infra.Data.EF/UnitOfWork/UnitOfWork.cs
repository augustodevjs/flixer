using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Infra.Data.EF.Context;

namespace Flixer.Catalog.Infra.Data.EF.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly FlixerCatalogDbContext _context;

    public UnitOfWork(FlixerCatalogDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Commit()
        => await _context.SaveChangesAsync() > 0;
}