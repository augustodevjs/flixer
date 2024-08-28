using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Domain.Contracts.Repository;

public interface ICategoryRepository : IRepository<Category>, ISearchableRepository<Category>
{
    public Task<IReadOnlyList<Guid>> GetIdsListByIds(List<Guid> ids);
    public Task<IReadOnlyList<Category>> GetListByIds(List<Guid> ids);
}