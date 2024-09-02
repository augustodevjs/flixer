using MediatR;

namespace Flixer.Catalog.Application.Common.Input.CastMember;

public class DeleteCastMemberInput : IRequest
{
    public Guid Id { get; private set; }

    public DeleteCastMemberInput(Guid id)
    {
        Id = id;
    }
}