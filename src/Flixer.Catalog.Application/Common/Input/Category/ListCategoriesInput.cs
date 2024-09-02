using MediatR;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Application.Common.Output;
using Flixer.Catalog.Application.Common.Input.Common;
using Flixer.Catalog.Application.Common.Output.Category;

namespace Flixer.Catalog.Application.Common.Input.Category;

public class ListCategoriesInput: PaginatedListInput, IRequest<ListCategoriesOutput>
{
    public ListCategoriesInput(
        int page = 1,
        int perPage = 15,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc
    ) : base(page, perPage, search, sort, dir)
    {
    }
    
    public ListCategoriesInput()
        : base(1, 15, "", "", SearchOrder.Asc)
    {
    }
}