using Flixer.Catalog.Domain.Enums;

namespace Flixer.Catalog.Application.Common.Output.CastMember;

public class CastMemberOutput
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public CastMemberType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public CastMemberOutput(
        Guid id,
        string name,
        CastMemberType type,
        DateTime createdAt
    )
    {
        Id = id;
        Name = name;
        Type = type;
        CreatedAt = createdAt;
    }

    public static CastMemberOutput FromCastMember(Domain.Entities.CastMember castMember)
        => new(
            castMember.Id,
            castMember.Name,
            castMember.Type,
            castMember.CreatedAt
        );
}