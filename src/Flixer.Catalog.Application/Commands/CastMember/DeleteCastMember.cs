using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.CastMember;

namespace Flixer.Catalog.Application.Commands.CastMember;

public class DeleteCastMember : IRequestHandler<DeleteCastMemberInput>
{
    private readonly ILogger<DeleteCastMember> _logger;
    private readonly ICastMemberRepository _castMemberRepository;

    public DeleteCastMember(
        ILogger<DeleteCastMember> logger, 
        ICastMemberRepository castMemberRepository
    )
    {
        _logger = logger;
        _castMemberRepository = castMemberRepository;
    }   
    
    public async Task Handle(DeleteCastMemberInput request, CancellationToken cancellationToken)
    {
        var castMember = await _castMemberRepository.GetById(request.Id);
        
        if (castMember == null)
            NotFoundException.ThrowIfNull(castMember, $"CastMember '{request.Id}' not found.");

        _castMemberRepository.Delete(castMember!);

        await _castMemberRepository.UnityOfWork.Commit();
        
        _logger.LogInformation("CastMember with ID: {CastMemberId} deleted successfully.", request.Id);
    }
}