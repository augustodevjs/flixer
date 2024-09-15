using Xunit; 
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.IntegrationTests.Fixtures.Repository;

[CollectionDefinition(nameof(CategoryRepositoryFixture))]
public class CategoryRepositoryFixtureCollection : ICollectionFixture<CategoryRepositoryFixture>
{
    
}

public class CategoryRepositoryFixture : BaseFixture
{
    public CategoryDataGenerator DataGenerator { get; } = new();
}