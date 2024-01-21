using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Infra.Data.EF.Context;
using DomainEntity = Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.EndToEndTests.Api.Category.Common;

public class CategoryPersistence
{
    private readonly FlixerCatalogDbContext _context;

    public CategoryPersistence(FlixerCatalogDbContext context)
    {
        _context = context;
    }

    public async Task<DomainEntity.Category?> GetById(Guid id)
    {
        return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task Insert(DomainEntity.Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }
}
