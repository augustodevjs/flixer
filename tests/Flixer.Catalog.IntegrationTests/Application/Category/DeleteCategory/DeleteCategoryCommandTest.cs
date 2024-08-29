using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Infra.Data.EF.Repositories;
using Flixer.Catalog.Application.Commands.Category.DeleteCategory;
using Flixer.Catalog.IntegrationTests.Fixtures.Repository;

namespace Flixer.Catalog.IntegrationTests.Application.Category.DeleteCategory;

[Collection(nameof(CategoryRepositoryFixture))]
public class DeleteCategoryCommandTest
{
    private readonly CategoryRepositoryFixture _fixture;
    private const string NameDbContext = "integration-tests-db";

    public DeleteCategoryCommandTest(CategoryRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("Integration/Application", "DeleteCategory - Command")]
    public async Task Command_DeleteCategory()
    {
        var categoryExample = _fixture.CategoryFixture.GetValidCategory();
        var dbContext = _fixture.CreateDbContext(NameDbContext, true);
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<DeleteCategoryCommandHandler>();

        var tracking = await dbContext.AddAsync(categoryExample);
        await dbContext.SaveChangesAsync();
        tracking.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);

        var command = new DeleteCategoryCommandHandler(repository, logger);

        var input = new DeleteCategoryCommand(categoryExample.Id);

        await command.Handle(input, CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(NameDbContext, true);
        var dbCategoryDeleted = await assertDbContext.Categories.FindAsync(categoryExample.Id);

        dbCategoryDeleted.Should().BeNull();
    }

    [Fact]
    [Trait("Integration/Application", "DeleteCategory - Command")]
    public async Task Command_DeleteCategoryThrowsWhenNotFound()
    {
        var dbContext = _fixture.CreateDbContext(NameDbContext);
        var exampleList = _fixture.ListCategoriesQueryFixture.GetExampleCategoriesList();
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<DeleteCategoryCommandHandler>();
    
        await dbContext.AddRangeAsync(exampleList);
        await dbContext.SaveChangesAsync();
    
        var repository = new CategoryRepository(dbContext);
    
        var command = new DeleteCategoryCommandHandler(repository, logger);
    
        var input = new DeleteCategoryCommand(Guid.NewGuid());
    
        var task = async () => await command.Handle(input, CancellationToken.None);
    
        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{input.Id}' not found.");
    }
}