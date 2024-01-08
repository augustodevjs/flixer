using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Domain.Repository;

public interface ICategoryRepository : IGenericRepository<Category>, ISearchableRepository<Category>
{
}