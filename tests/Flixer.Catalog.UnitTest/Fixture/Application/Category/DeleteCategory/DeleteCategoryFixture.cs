using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.UnitTest.Fixture.Domain;
using Flixer.Catalog.Domain.Contracts.Repository;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryFixture))]
public class DeleteCategoryCommandFixtureCollection : ICollectionFixture<DeleteCategoryFixture>
{
    
}

public class DeleteCategoryFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Commands.Category.DeleteCategory>> GetLoggerMock() => new();
}