using Flixer.Catalog.Application.Dtos.Common;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Application.Dtos.ViewModel.Category;

public class ListCategoriesViewModel : PaginatedListOutput<CategoryViewModel>
{
    public ListCategoriesViewModel(
       int page,
       int perPage,
       int total,
       IReadOnlyList<CategoryViewModel> items)
       : base(page, perPage, total, items)
    {
    }

    public static ListCategoriesViewModel FromSearchOutput(SearchOutput<DomainEntity.Category> searchOutput)
        => new(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(CategoryViewModel.FromCategory)
                .ToList()
        );
}
