using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;
using Flixer.Catalog.Infra.Data.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace Flixer.Catalog.Infra.Data.EF.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly FlixerCatalogDbContext _context;
    private DbSet<Category> _categories => _context.Set<Category>();

    public CategoryRepository(FlixerCatalogDbContext context)
    {
        _context = context;
    }

    public async Task Insert(Category aggregate, CancellationToken cancellationToken)
    {
        await _categories.AddAsync(aggregate, cancellationToken);
    }

    public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return category!;
    }
    public Task Update(Category aggregate, CancellationToken _)
        => Task.FromResult(_categories.Update(aggregate));

    public Task Delete(Category aggregate, CancellationToken cancellationToken)
        => Task.FromResult(_categories.Remove(aggregate));

    public Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Guid>> GetIdsListByIds(List<Guid> ids, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
