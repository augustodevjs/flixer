using MediatR;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Application.Common.Input.Common;
using Flixer.Catalog.Application.Common.Output.CastMember;

namespace Flixer.Catalog.Application.Common.Input.CastMember;

public class ListCastMembersInput : PaginatedListInput, IRequest<ListCastMembersOutput>
{
    public ListCastMembersInput() : base(1, 10, "", "", SearchOrder.Asc)
    {

    }

    public ListCastMembersInput(
        int page,
        int perPage,
        string search,
        string sort,
        SearchOrder dir) : base(page, perPage, search, sort, dir)
    {
    }
}