using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.UnitTest.Common;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Queries.Category;
using Flixer.Catalog.Application.Queries.Category.GetCategory;
using Flixer.Catalog.UnitTest.Domain.Fixtures.Category;

namespace Flixer.Catalog.UnitTest.Application.Fixtures.Category.GetCategory;

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