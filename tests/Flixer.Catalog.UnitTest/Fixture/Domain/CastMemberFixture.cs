using Xunit;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Domain;

[CollectionDefinition(nameof(CastMemberFixture))]
public class CastMemberFixtureCollection : ICollectionFixture<CastMemberFixture>
{
    
}

public class CastMemberFixture
{
    public CastMemberDataGenerator DataGenerator { get; } = new();
}