using Flixer.Catalog.Domain.Enums;

namespace Flixer.Catalog.Api.ApiModels.CastMember;

public class UpdateCastMemberApiInput
{
    public string Name { get; set; }
    public CastMemberType Type { get; set; }

    public UpdateCastMemberApiInput(string name, CastMemberType type)
    {
        Name = name;
        Type = type;
    }
}