using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Infra.Data.EF.Context;
using Flixer.Catalog.Infra.Data.EF.Abstractions;
using Flixer.Catalog.Domain.Contracts.Repository;

namespace Flixer.Catalog.Infra.Data.EF.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(FlixerCatalogDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Guid>> GetIdsListByIds(List<Guid> ids)
    {
        return await Context.Categories
            .AsNoTracking()
            .Where(category => ids.Contains(category.Id))
            .Select(category => category.Id)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Category>> GetListByIds(List<Guid> ids)
    {
        return await Context.Categories
            .AsNoTracking()
            .Where(category => ids.Contains(category.Id))
            .ToListAsync();
    }
}