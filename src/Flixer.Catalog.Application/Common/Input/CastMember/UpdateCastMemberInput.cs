using MediatR;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Application.Common.Output.CastMember;

namespace Flixer.Catalog.Application.Common.Input.CastMember;

public class UpdateCastMemberInput : IRequest<CastMemberOutput>
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public CastMemberType Type { get; private set; }
    
    public UpdateCastMemberInput(Guid id, string name, CastMemberType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }
}