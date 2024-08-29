using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.UnitTest.Fixture.Domain.Category;
using Flixer.Catalog.Application.Queries.Category.GetCategory;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryQueryFixture))]
public class GetCategoryQueryFixtureCollection : ICollectionFixture<GetCategoryQueryFixture>
{
    
}

public class GetCategoryQueryFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<GetCategoryQueryHandler>> GetLoggerMock() => new();
}