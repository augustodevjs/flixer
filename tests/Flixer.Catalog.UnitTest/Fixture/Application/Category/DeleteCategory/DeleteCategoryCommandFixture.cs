using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Commands.Category.DeleteCategory;
using Flixer.Catalog.UnitTest.Fixture.Domain;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryCommandFixture))]
public class DeleteCategoryCommandFixtureCollection : ICollectionFixture<DeleteCategoryCommandFixture>
{
    
}

public class DeleteCategoryCommandFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<DeleteCategoryCommandHandler>> GetLoggerMock() => new();
}