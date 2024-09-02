using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryFixture))]
public class GetCategoryQueryFixtureCollection : ICollectionFixture<GetCategoryFixture>
{
    
}

public class GetCategoryFixture
{
    public CategoryDataGenerator DataGenerator { get; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Queries.Category.GetCategory>> GetLoggerMock() => new();
}