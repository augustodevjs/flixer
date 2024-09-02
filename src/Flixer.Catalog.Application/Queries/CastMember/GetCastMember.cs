using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.CastMember;
using Flixer.Catalog.Application.Common.Output.CastMember;

namespace Flixer.Catalog.Application.Queries.CastMember;

public class GetCastMember : IRequestHandler<GetCastMemberInput, CastMemberOutput>
{
    private readonly ILogger<GetCastMember> _logger;
    private readonly ICastMemberRepository _castMemberRepository;

    public GetCastMember(
        ILogger<GetCastMember> logger, 
        ICastMemberRepository castMemberRepository
    )
    {
        _logger = logger;
        _castMemberRepository = castMemberRepository;
    }
    
    public async Task<CastMemberOutput> Handle(GetCastMemberInput request, CancellationToken cancellationToken)
    {
        var castMember = await _castMemberRepository.GetById(request.Id);

        if (castMember == null)
            NotFoundException.ThrowIfNull(castMember, $"CastMember '{request.Id}' not found.");

        _logger.LogInformation("CastMember with ID: {CastMemberId} retrieved successfully.", request.Id);

        return CastMemberOutput.FromCastMember(castMember!);
    }
}