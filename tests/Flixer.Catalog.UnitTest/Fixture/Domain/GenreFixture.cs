using Xunit;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Domain;

[CollectionDefinition(nameof(GenreFixture))]
public class GenreFixtureCollection : ICollectionFixture<GenreFixture>
{
    
}

public class GenreFixture 
{
    public GenreDataGenerator DataGenerator { get; } = new();
}