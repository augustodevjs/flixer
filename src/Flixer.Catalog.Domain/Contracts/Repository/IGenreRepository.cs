using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Domain.Contracts.Repository;

public interface IGenreRepository : IRepository<Genre>, ISearchableRepository<Genre>
{
    public Task<IReadOnlyList<Genre>> GetListByIdsAsync(List<Guid> ids);
    public Task<IReadOnlyList<Guid>> GetIdsListByIdsAsync(List<Guid> ids);
}