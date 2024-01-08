using Flixer.Catalog.Application.Common;
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
}
