using MediatR;
using Flixer.Catalog.Application.Common.Output.CastMember;

namespace Flixer.Catalog.Application.Common.Input.CastMember;

public class GetCastMemberInput : IRequest<CastMemberOutput>
{
    public Guid Id { get; private set; }

    public GetCastMemberInput(Guid id)
    {
        Id = id;
    }
}