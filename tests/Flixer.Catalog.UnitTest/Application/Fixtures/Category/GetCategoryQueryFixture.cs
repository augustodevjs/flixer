using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.UnitTest.Common;
using Flixer.Catalog.UnitTest.Domain.Fixtures.Category;

namespace Flixer.Catalog.UnitTest.Application.Fixtures.Category;

[CollectionDefinition(nameof(GetCategoryQueryFixtureCollection))]
public class GetCategoryQueryFixtureCollection : ICollectionFixture<GetCategoryQueryFixture>
{
    
}

public class GetCategoryQueryFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; set; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock { get; set; } = new();
}