namespace Flixer.Catalog.Application.Common.Output.Common;

public abstract class PaginatedListOutput<TOutputItem>
{
    public int Page { get; set; }
    public int Total { get; set; }
    public int PerPage { get; set; }
    public IReadOnlyList<TOutputItem> Items { get; set; }
    
    public PaginatedListOutput(
        int page, 
        int total, 
        int perPage, 
        IReadOnlyList<TOutputItem> items
    )
    {
        Page = page;
        Total = total;
        PerPage = perPage;
        Items = items;
    }
}