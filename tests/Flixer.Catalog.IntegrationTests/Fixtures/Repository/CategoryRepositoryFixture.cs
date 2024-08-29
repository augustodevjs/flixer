using Xunit;
using Flixer.Catalog.UnitTest.Fixture.Domain.Category;

namespace Flixer.Catalog.IntegrationTests.Fixtures.Repository;

[CollectionDefinition(nameof(CategoryRepositoryFixture))]
public class CategoryRepositoryFixtureCollection : ICollectionFixture<CategoryRepositoryFixture>
{
    
}

public class CategoryRepositoryFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; } = new();
    
    
}