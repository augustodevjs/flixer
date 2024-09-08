using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Tests.Shared.DataGenerators;

public class CastMemberDataGenerator : DataGeneratorBase
{
    public string GetValidName()
        => Faker.Name.FullName();

    public CastMemberType GetRandomCastMemberType()
        => (CastMemberType)new Random().Next(1, 2);

    public CastMember GetValidCastMember()
        => new(GetValidName(), GetRandomCastMemberType());
}