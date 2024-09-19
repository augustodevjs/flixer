using MediatR;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Application.Common.Input.Common;
using Flixer.Catalog.Application.Common.Output.Video;

namespace Flixer.Catalog.Application.Common.Input.Video;

public class ListVideosInput  : PaginatedListInput, IRequest<ListVideosOutput>
{
    public ListVideosInput(
        int page = 1,
        int perPage = 15,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc
    ) 
        : base(page, perPage, search, sort, dir)
    { }
}