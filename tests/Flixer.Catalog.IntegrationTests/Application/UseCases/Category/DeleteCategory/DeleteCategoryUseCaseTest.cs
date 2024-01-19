using Flixer.Catalog.Application.Dtos.InputModel.Category;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Infra.Data.EF.Context;
using Flixer.Catalog.Infra.Data.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using ApplicationUseCase = Flixer.Catalog.Application.UseCases.Category;

namespace Flixer.Catalog.IntegrationTests.Application.UseCases.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryUseCaseTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryUseCaseTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Integration/Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategory()
    {
        var dbContext = _fixture.CreateDbContext(true);
        var categoryExample = _fixture.GetExampleCategory();

        var tracking = await dbContext.AddAsync(categoryExample);
        await dbContext.SaveChangesAsync();
        tracking.State = EntityState.Detached;

        var unityOfWork = new UnityOfWork(dbContext);
        var repository = new CategoryRepository(dbContext);

        var useCase = new ApplicationUseCase.DeleteCategory(repository, unityOfWork);

        var input = new DeleteCategoryInputModel(categoryExample.Id);

        await useCase.Handle(input, CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(true);
        var dbCategoryDeleted = await assertDbContext.Categories.FindAsync(categoryExample.Id);

        dbCategoryDeleted.Should().BeNull();
    }

    [Fact(DisplayName = nameof(DeleteCategoryThrowsWhenNotFound))]
    [Trait("Integration/Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategoryThrowsWhenNotFound()
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleList = _fixture.GetExampleCategoriesList(10);

        await dbContext.AddRangeAsync(exampleList);
        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnityOfWork(dbContext);

        var useCase = new ApplicationUseCase.DeleteCategory(repository, unitOfWork);

        var input = new DeleteCategoryInputModel(Guid.NewGuid());

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{input.Id}' not found.");
    }
}