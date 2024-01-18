using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Domain.Repository;

public interface ICategoryRepository : IGenericRepository<Category>, ISearchableRepository<Category>
{
    public Task<IReadOnlyList<Category>> GetListByIds(List<Guid> ids, CancellationToken cancellationToken);
    public Task<IReadOnlyList<Guid>> GetIdsListByIds(List<Guid> ids, CancellationToken cancellationToken);
}