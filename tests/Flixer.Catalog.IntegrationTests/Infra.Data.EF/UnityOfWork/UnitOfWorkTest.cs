using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Infra.Data.EF.Repositories;
using Flixer.Catalog.IntegrationTests.Fixtures.Repository;

namespace Flixer.Catalog.IntegrationTests.Infra.Data.EF.UnityOfWork;

[Collection(nameof(CategoryRepositoryFixture))]
public class UnitOfWorkTest
{
    private readonly CategoryRepositoryFixture _fixture;

    public UnitOfWorkTest(CategoryRepositoryFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Repository_ShouldCommit()
    {
        var dbContext = _fixture.CreateDbContext("integrations-tests-repository");
        var exampleCategoriesList = _fixture.ListCategoriesFixture.GetExampleCategoriesList();
        
        await dbContext.AddRangeAsync(exampleCategoriesList);
        
        var categoryRepository = new CategoryRepository(dbContext);

        await categoryRepository.UnityOfWork.Commit();

        var assertDbContext = _fixture.CreateDbContext("integrations-tests-repository", true);
        
        var savedCategories = assertDbContext.Categories
            .AsNoTracking().ToList();
        
        savedCategories.Should()
            .HaveCount(exampleCategoriesList.Count);
    }
    
    [Fact]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Repository_ShouldNotCommit()
    {
        var dbContext = _fixture.CreateDbContext("integrations-tests-repository");
        var exampleCategoriesList = _fixture.ListCategoriesFixture.GetExampleCategoriesList();
        
        await dbContext.AddRangeAsync(exampleCategoriesList);

        var assertDbContext = _fixture.CreateDbContext("integrations-tests-repository", true);
        
        var savedCategories = assertDbContext.Categories
            .AsNoTracking().ToList();

        savedCategories.Should().BeEmpty();
    }
}