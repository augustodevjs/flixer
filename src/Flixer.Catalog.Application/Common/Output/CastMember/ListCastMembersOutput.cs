using Flixer.Catalog.Application.Common.Output.Common;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Application.Common.Output.CastMember;

public class ListCastMembersOutput : PaginatedListOutput<CastMemberOutput>
{
    public ListCastMembersOutput(
        int currentPage,
        int perPage,
        IReadOnlyList<CastMemberOutput> items,
        int total) : base(currentPage, total, perPage, items)
    {
    }

    public static ListCastMembersOutput FromSearchOutput(SearchOutput<Domain.Entities.CastMember> searchOutput)
        => new(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Items
                .Select(castMember =>
                    CastMemberOutput.FromCastMember(castMember))
                .ToList()
                .AsReadOnly(),
            searchOutput.Total
        );
}