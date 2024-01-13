using Flixer.Catalog.Infra.Data.EF.Context;
using Repository = Flixer.Catalog.Infra.Data.EF.Repositories;

namespace Flixer.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryTestFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "categoryRepository - Repositories")]
    public async Task Insert()
    {
        FlixerCatalogDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategory = _fixture.GetExampleCategory();
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await categoryRepository.Insert(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbCategory = await dbContext.Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(exampleCategory.Name);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        dbCategory.Description.Should().Be(exampleCategory.Description);
    }

    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "categoryRepository - Repositories")]
    public async Task Get()
    {
        FlixerCatalogDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategory = _fixture.GetExampleCategory();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);

        exampleCategoriesList.Add(exampleCategory);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.CategoryRepository(
            _fixture.CreateDbContext(true)
        );

        var dbCategory = await categoryRepository.Get(exampleCategory.Id, CancellationToken.None);

        dbCategory.Should().NotBeNull();
        dbCategory.Id.Should().Be(exampleCategory.Id);
        dbCategory!.Name.Should().Be(exampleCategory.Name);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        dbCategory.Description.Should().Be(exampleCategory.Description);
    }
}
