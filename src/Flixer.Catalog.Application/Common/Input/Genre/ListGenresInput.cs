using MediatR;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Application.Common.Output;
using Flixer.Catalog.Application.Common.Input.Common;
using Flixer.Catalog.Application.Common.Output.Genre;

namespace Flixer.Catalog.Application.Common.Input.Genre;

public class ListGenresInput : PaginatedListInput, IRequest<ListGenresOutput>
{
    public ListGenresInput(
        int page = 1,
        int perPage = 15,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc
    )
        : base(page, perPage, search, sort, dir)
    { }
}