using MediatR;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Application.Common;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;
using Flixer.Catalog.Application.Commands.Category.CreateCategory;

namespace Flixer.Catalog.Application.Queries.Category.ListCategories;

public class ListCategoriesQuery : PaginatedListInput, IRequest<ListCategoriesOutput>
{
    public ListCategoriesQuery(
        int page = 1,
        int perPage = 15,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc
    ) : base(page, perPage, search, sort, dir)
    {
    }
    
    public ListCategoriesQuery()
        : base(1, 15, "", "", SearchOrder.Asc)
    {
    }
}

public class ListCategoriesOutput : PaginatedListOutput<CategoryViewModel>
{
    public ListCategoriesOutput(
        int page, 
        int total, 
        int perPage, 
        IReadOnlyList<CategoryViewModel> items
    ) : base(page, total, perPage, items)
    {
    }
    
    public static ListCategoriesOutput FromSearchOutput(SearchOutput<Domain.Entities.Category> searchOutput)
        => new(
            searchOutput.CurrentPage,
            searchOutput.Total,
            searchOutput.PerPage,
            searchOutput.Items
                .Select(CategoryViewModel.FromCategory)
                .ToList()
        );
}