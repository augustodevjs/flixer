namespace Flixer.Catalog.Domain.SeedWork.SearchableRepository;

public class SearchOutput<TAggregate> where TAggregate : AggregateRoot
{
    public int Total { get; private set; }
    public int PerPage { get; private set; }
    public int CurrentPage { get; private set; }
    public IReadOnlyList<TAggregate> Items { get; private set; }
    
    public SearchOutput(
        int total, 
        int perPage, 
        int currentPage, 
        IReadOnlyList<TAggregate> items
    )
    {
        Total = total;
        Items = items;
        PerPage = perPage;
        CurrentPage = currentPage;
    }
}