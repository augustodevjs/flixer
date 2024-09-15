using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.CastMember;
using Flixer.Catalog.Application.Common.Output.CastMember;

namespace Flixer.Catalog.Application.Commands.CastMember;

public class UpdateCastMember : IRequestHandler<UpdateCastMemberInput, CastMemberOutput>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCastMember> _logger;
    private readonly ICastMemberRepository _castMemberRepository;

    public UpdateCastMember(
        IUnitOfWork unityOfWork,
        ILogger<UpdateCastMember> logger, 
        ICastMemberRepository castMemberRepository
    )
    {
        _logger = logger;
        _unitOfWork = unityOfWork;
        _castMemberRepository = castMemberRepository;
    }   
    
    public async Task<CastMemberOutput> Handle(UpdateCastMemberInput request, CancellationToken cancellationToken)
    {
        try
        {
            var castMember = await _castMemberRepository.GetById(request.Id);

            if (castMember == null)
                NotFoundException.ThrowIfNull(castMember, $"CastMember '{request.Id}' not found.");

            castMember!.Update(request.Name, request.Type);

            _castMemberRepository.Update(castMember);

            await _unitOfWork.Commit();

            _logger.LogInformation("CastMember with ID: {CastMemberId} updated successfully.", request.Id);

            return CastMemberOutput.FromCastMember(castMember);
        }
        catch (EntityValidationException ex)
        {
            _logger.LogError(ex, "Validation error occurred while updating cast member with ID: {CastMemberId}", request.Id);
            throw new EntityValidationException(ex.Message, ex.Errors);
        }
    }
}