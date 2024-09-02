using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;
using Flixer.Catalog.Application.Queries.Category;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.ListCategory;

[CollectionDefinition(nameof(ListCategoriesFixture))]
public class ListCategoryQueryFixtureCollection : ICollectionFixture<ListCategoriesFixture>
{
    
}

public class ListCategoriesFixture
{
    public CategoryDataGenerator DataGenerator { get; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<ListCategories>> GetLogger() => new();
}