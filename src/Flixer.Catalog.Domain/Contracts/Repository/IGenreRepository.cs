using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Domain.Contracts.Repository;

public interface IGenreRepository : IRepository<Genre>, ISearchableRepository<Genre>
{
    
}