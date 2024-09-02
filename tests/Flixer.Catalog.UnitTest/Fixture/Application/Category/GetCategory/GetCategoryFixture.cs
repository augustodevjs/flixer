using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.UnitTest.Fixture.Domain;
using Flixer.Catalog.Domain.Contracts.Repository;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryFixture))]
public class GetCategoryQueryFixtureCollection : ICollectionFixture<GetCategoryFixture>
{
    
}

public class GetCategoryFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Queries.Category.GetCategory>> GetLoggerMock() => new();
}