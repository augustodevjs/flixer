using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.UnitTest.Common;
using Flixer.Catalog.UnitTest.Domain.Fixtures.Category;

namespace Flixer.Catalog.UnitTest.Application.Fixtures.Category;

[CollectionDefinition(nameof(DeleteCategoryCommandFixtureCollection))]
public class DeleteCategoryCommandFixtureCollection : ICollectionFixture<DeleteCategoryCommandFixture>
{
    
}

public class DeleteCategoryCommandFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; set; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock { get; set; } = new();
}