using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Application.Common;

public abstract class PaginatedListInput
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public string Sort { get; set; }
    public string Search { get; set; }
    public SearchOrder Dir { get; set; }
    
    public PaginatedListInput(
        int page, 
        int perPage, 
        string search, 
        string sort, 
        SearchOrder dir
    )
    {
        Dir = dir;
        Sort = sort;
        Page = page;
        Search = search;
        PerPage = perPage;
    }
    
    public SearchInput ToSearchInput()
        => new(Page, PerPage, Search, Sort, Dir);
}