using Flixer.Catalog.Application.Common.Output.Common;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Application.Common.Output.Category;

public class ListCategoriesOutput : PaginatedListOutput<CategoryOutput>
{
    public ListCategoriesOutput(
        int page, 
        int total, 
        int perPage, 
        IReadOnlyList<CategoryOutput> items
    ) : base(page, total, perPage, items)
    {
    }
    
    public static ListCategoriesOutput FromSearchOutput(SearchOutput<Domain.Entities.Category> searchOutput)
        => new(
            searchOutput.CurrentPage,
            searchOutput.Total,
            searchOutput.PerPage,
            searchOutput.Items
                .Select(CategoryOutput.FromCategory)
                .ToList()
        );
}