using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryFixture))]
public class CreateCategoryCommandFixtureCollection : ICollectionFixture<CreateCategoryFixture>
{
    
}

public class CreateCategoryFixture
{
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
    public CategoryDataGenerator DataGenerator { get; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Commands.Category.CreateCategory>> GetLoggerMock() => new();
}