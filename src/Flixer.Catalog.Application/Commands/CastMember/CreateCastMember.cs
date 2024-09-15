using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.CastMember;
using Flixer.Catalog.Application.Common.Output.CastMember;

namespace Flixer.Catalog.Application.Commands.CastMember;

public class CreateCastMember : IRequestHandler<CreateCastMemberInput, CastMemberOutput>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCastMember> _logger;
    private readonly ICastMemberRepository _castMemberRepository;

    public CreateCastMember(
        IUnitOfWork unitOfWork, 
        ILogger<CreateCastMember> logger, 
        ICastMemberRepository castMemberRepository
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _castMemberRepository = castMemberRepository;
    }

    public async Task<CastMemberOutput> Handle(CreateCastMemberInput request, CancellationToken cancellationToken)
    {
        try
        {
            var castMember = new Domain.Entities.CastMember(request.Name, request.Type);
            
            _castMemberRepository.Create(castMember);

            await _unitOfWork.Commit();
            
            _logger.LogInformation("CastMember created successfully with ID: {CastMemberId}", castMember.Id);

            return CastMemberOutput.FromCastMember(castMember);
        }
        catch (EntityValidationException ex)
        {
            _logger.LogError(ex, "Validation error occurred while creating cast member with Name: {Name}", request.Name);
            throw new EntityValidationException(ex.Message, ex.Errors);
        }
    }
}