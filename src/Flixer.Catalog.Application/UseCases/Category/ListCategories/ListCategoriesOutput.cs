using Flixer.Catalog.Application.Common;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;
using Flixer.Catalog.Application.UseCases.Category.Common;

namespace Flixer.Catalog.Application.UseCases.Category.ListCategories;

public class ListCategoriesOutput : PaginatedListOutput<CategoryModelOutput>
{
    public ListCategoriesOutput(
       int page,
       int perPage,
       int total,
       IReadOnlyList<CategoryModelOutput> items)
       : base(page, perPage, total, items)
    {
    }

    public static ListCategoriesOutput FromSearchOutput(SearchOutput<DomainEntity.Category> searchOutput)
        => new(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(CategoryModelOutput.FromCategory)
                .ToList()
        );
}
