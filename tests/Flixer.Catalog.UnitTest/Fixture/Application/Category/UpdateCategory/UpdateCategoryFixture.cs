using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryFixture))]
public class UpdateCategoryCommandFixtureCollection : ICollectionFixture<UpdateCategoryFixture>
{
    
}

public class UpdateCategoryFixture
{
    public CategoryDataGenerator DataGenerator { get; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Commands.Category.UpdateCategory>> GetLoggerMock() => new();
}