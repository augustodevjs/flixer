using MediatR;
using Flixer.Catalog.Application.Dtos.Common;
using Flixer.Catalog.Application.Dtos.ViewModel.Genre;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Application.Dtos.InputModel.Genre;

public class ListGenresInputModel : PaginatedListInput, IRequest<ListGenresViewModel>
{
    public ListGenresInputModel(
        int page = 1,
        int perPage = 15,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc
    )
        : base(page, perPage, search, sort, dir)
    {
    }
}