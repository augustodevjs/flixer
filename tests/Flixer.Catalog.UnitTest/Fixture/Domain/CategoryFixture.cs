using Xunit;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Domain;

[CollectionDefinition(nameof(CategoryFixture))]
public class CategoryFixtureCollection : ICollectionFixture<CategoryFixture>
{
    
}

public class CategoryFixture 
{
    public CategoryDataGenerator DataGenerator { get; } = new();
}