using Microsoft.Extensions.Logging;
using Flixer.Catalog.UnitTest.Common;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.UnitTest.Domain.Fixtures.Category;
using Flixer.Catalog.Application.Commands.Category.DeleteCategory;

namespace Flixer.Catalog.UnitTest.Application.Fixtures.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryCommandFixtureCollection))]
public class DeleteCategoryCommandFixtureCollection : ICollectionFixture<DeleteCategoryCommandFixture>
{
    
}

public class DeleteCategoryCommandFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<DeleteCategoryCommandHandler>> GetLoggerMock() => new();
}