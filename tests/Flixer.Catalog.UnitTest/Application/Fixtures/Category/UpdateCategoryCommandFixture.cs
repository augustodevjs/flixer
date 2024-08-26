using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.UnitTest.Common;
using Flixer.Catalog.UnitTest.Domain.Fixtures.Category;

namespace Flixer.Catalog.UnitTest.Application.Fixtures.Category;

[CollectionDefinition(nameof(UpdateCategoryCommandFixtureCollection))]
public class UpdateCategoryCommandFixtureCollection : ICollectionFixture<UpdateCategoryCommandFixture>
{
    
}

public class UpdateCategoryCommandFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; set; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock { get; set; } = new();
}