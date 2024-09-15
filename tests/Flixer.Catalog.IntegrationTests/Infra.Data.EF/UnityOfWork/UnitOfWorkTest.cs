using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Infra.Data.EF.UnitOfWork;
using Flixer.Catalog.IntegrationTests.Fixtures.Repository;

namespace Flixer.Catalog.IntegrationTests.Infra.Data.EF.UnityOfWork;

[Collection(nameof(CategoryRepositoryFixture))]
public class UnitOfWorkTest
{
    private readonly CategoryRepositoryFixture _fixture;
    private const string DatabaseName = "integration-tests-repository"; 

    public UnitOfWorkTest(CategoryRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Repository_ShouldCommit()
    {
        var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList();

        var dbContext = _fixture.CreateDbContext(DatabaseName);
        await dbContext.AddRangeAsync(exampleCategoriesList);
        var unitOfWork = new UnitOfWork(dbContext);

        await unitOfWork.Commit();

        var assertDbContext = _fixture.CreateDbContext(DatabaseName, true);
        var savedCategories = assertDbContext.Categories.AsNoTracking().ToList();

        savedCategories.Should().HaveCount(exampleCategoriesList.Count);
    }

    [Fact]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Repository_ShouldNotCommit()
    {
        var dbContext = _fixture.CreateDbContext(DatabaseName);
        var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList();

        await dbContext.AddRangeAsync(exampleCategoriesList);

        var assertDbContext = _fixture.CreateDbContext(DatabaseName, true);

        var savedCategories = assertDbContext.Categories
            .AsNoTracking().ToList();

        savedCategories.Should().BeEmpty();
    }
}