using Xunit;
using Flixer.Catalog.EndToEndTests.Persistence;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.EndToEndTests.Fixtures.Category;

[CollectionDefinition(nameof(CategoryFixture))]
public class CategoryFixtureCollection : ICollectionFixture<CategoryFixture>
{
    
}

public class CategoryFixture : BaseFixture
{
    public CategoryPersistence Persistence { get; }
    public CategoryDataGenerator DataGenerator { get; } = new();

    public CategoryFixture()
    {
        Persistence = new CategoryPersistence(
            CreateDbContext()
        );
    }
}