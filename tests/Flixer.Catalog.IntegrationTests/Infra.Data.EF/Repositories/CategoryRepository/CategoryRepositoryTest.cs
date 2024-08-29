using Xunit;
using FluentAssertions;
using Flixer.Catalog.IntegrationTests.Fixtures.Repository;

namespace Flixer.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryFixture))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task CategoryRepository_CreateCategory()
    {
        var dbContext = _fixture.CreateDbContext("integration-tests-repository");
        var exampleCategory = _fixture.CategoryFixture.GetValidCategory();
        var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);

        categoryRepository.Create(exampleCategory);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbCategory = await dbContext.Categories.FindAsync(exampleCategory.Id);
        
        dbCategory.Should().NotBeNull();
        dbCategory?.Name.Should().Be(exampleCategory.Name);
        dbCategory?.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory?.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        dbCategory?.Description.Should().Be(exampleCategory.Description);
    }
    
    [Fact]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task CategoryRepository_GetById()
    {
        var dbContext = _fixture.CreateDbContext("integration-tests-repository");
        var exampleCategory = _fixture.CategoryFixture.GetValidCategory();

        await dbContext.Categories.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);

        var dbCategory = await categoryRepository.GetById(exampleCategory.Id);
        
        dbCategory.Should().NotBeNull();
        dbCategory?.Id.Should().Be(exampleCategory.Id);
        dbCategory?.Name.Should().Be(exampleCategory.Name);
        dbCategory?.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory?.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        dbCategory?.Description.Should().Be(exampleCategory.Description);
    }
}