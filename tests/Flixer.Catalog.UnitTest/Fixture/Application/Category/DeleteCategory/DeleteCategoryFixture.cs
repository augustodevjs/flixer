using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryFixture))]
public class DeleteCategoryCommandFixtureCollection : ICollectionFixture<DeleteCategoryFixture>
{
    
}

public class DeleteCategoryFixture
{
    public CategoryDataGenerator DataGenerator { get; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Commands.Category.DeleteCategory>> GetLoggerMock() => new();
}