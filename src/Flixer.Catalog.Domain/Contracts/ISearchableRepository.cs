using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Domain.Contracts;

public interface ISearchableRepository<TAggregate> where TAggregate : AggregateRoot
{
    Task<SearchOutput<TAggregate>> Search(SearchInput input);
}