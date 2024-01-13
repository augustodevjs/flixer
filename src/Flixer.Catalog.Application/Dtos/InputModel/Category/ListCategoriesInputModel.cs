using MediatR;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;
using Flixer.Catalog.Application.Dtos.Common;

namespace Flixer.Catalog.Application.Dtos.InputModel.Category;

public class ListCategoriesInputModel : PaginatedListInput, IRequest<ListCategoriesViewModel>
{
    public ListCategoriesInputModel(
        int page = 1,
        int perPage = 15,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc
    ) : base(page, perPage, search, sort, dir)
    {
    }

    public ListCategoriesInputModel()
        : base(1, 15, "", "", SearchOrder.Asc)
    {
    }
}