using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.CastMember;
using Flixer.Catalog.Application.Common.Output.CastMember;

namespace Flixer.Catalog.Application.Queries.CastMember;

public class ListCastMember : IRequestHandler<ListCastMembersInput, ListCastMembersOutput>
{
    private readonly ILogger<ListCastMember> _logger;
    private readonly ICastMemberRepository _castMemberRepository;

    public ListCastMember(
        ILogger<ListCastMember> logger, 
        ICastMemberRepository castMemberRepository
    )
    {
        _logger = logger;
        _castMemberRepository = castMemberRepository;
    }
    
    public async Task<ListCastMembersOutput> Handle(ListCastMembersInput request, CancellationToken cancellationToken)
    {
        var searchInput = request.ToSearchInput();
        var searchOutput = await _castMemberRepository.Search(searchInput);
        
        _logger.LogInformation("Search completed successfully with {TotalItems} total items.", searchOutput.Total);

        return ListCastMembersOutput.FromSearchOutput(searchOutput);
    }
}