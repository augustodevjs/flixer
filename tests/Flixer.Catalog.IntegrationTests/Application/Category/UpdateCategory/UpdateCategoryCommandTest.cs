using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Infra.Data.EF.Repositories;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.IntegrationTests.Fixtures.Repository;
using Flixer.Catalog.Application.Commands.Category.UpdateCategory;
using Flixer.Catalog.UnitTest.Fixture.Application.Category.UpdateCategory;

namespace Flixer.Catalog.IntegrationTests.Application.Category.UpdateCategory;

[Collection(nameof(CategoryRepositoryFixture))]
public class UpdateCategoryCommandTest
{
    private readonly CategoryRepositoryFixture _fixture;
    private const string NameDbContext = "integration-tests-db";

    public UpdateCategoryCommandTest(CategoryRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [Trait("Integration/Application", "UpdateCategory - Command")]
    [MemberData(
         nameof(DataGenerator.GetCategoriesToUpdate),
         parameters: 5,
         MemberType = typeof(DataGenerator)
     )]
    public async Task Command_UpdateCategory(
         DomainEntity.Category exampleCategory,
         UpdateCategoryCommand input
     )
    {
        var dbContext = _fixture.CreateDbContext(NameDbContext);
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<UpdateCategoryCommandHandler>();

        var trackingInfo = await dbContext.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);
        var command = new UpdateCategoryCommandHandler(repository, logger);

        var output = await command.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(NameDbContext, true)).Categories.FindAsync(output.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)input.IsActive!);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);
    }

    [Theory]
    [Trait("Integration/Application", "UpdateCategory - Command")]
    [MemberData(
        nameof(DataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(DataGenerator)
    )]
    public async Task Command_UpdateCategoryWithoutIsActive(
        DomainEntity.Category exampleCategory,
        UpdateCategoryCommand exampleInput
    )
    {
        var input = new UpdateCategoryCommand(
            exampleInput.Id,
            exampleInput.Name,
            exampleInput.Description!
        );
        
        var dbContext = _fixture.CreateDbContext(NameDbContext);
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<UpdateCategoryCommandHandler>();
        
        await dbContext.AddRangeAsync(_fixture.ListCategoriesQueryFixture.GetExampleCategoriesList());
        
        var trackingInfo = await dbContext.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();
        
        trackingInfo.State = EntityState.Detached;
        var repository = new CategoryRepository(dbContext);
        
        var command = new UpdateCategoryCommandHandler(repository, logger);
    
        var output = await command.Handle(input, CancellationToken.None);
    
        var dbCategory = await _fixture.CreateDbContext(NameDbContext, true)
            .Categories.FindAsync(output.Id);
        
        output.Should().NotBeNull();
        dbCategory.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        dbCategory!.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
    }
    
    [Fact]
    [Trait("Integration/Application", "UpdateCategory - Command")]
    public async Task Command_UpdateThrowsWhenNotFoundCategory()
    {
        var input = _fixture.UpdateCategoryCommandFixture.GetInputUpdate();
        var dbContext = _fixture.CreateDbContext(NameDbContext);
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<UpdateCategoryCommandHandler>();
        
        await dbContext.AddRangeAsync(_fixture.ListCategoriesQueryFixture.GetExampleCategoriesList());
        await dbContext.SaveChangesAsync();
        
        var repository = new CategoryRepository(dbContext);
        var command = new UpdateCategoryCommandHandler(repository, logger);
    
        var task = async () => await command.Handle(input, CancellationToken.None);
    
        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{input.Id}' not found.");
    }
    
    [Theory]
    [Trait("Integration/Application", "UpdateCategory - Command")]
    [MemberData(
        nameof(DataGenerator.GetInvalidUpdateInputs),
        parameters: 6,
        MemberType = typeof(DataGenerator)
    )]
    public async Task Command_UpdateThrowsWhenCantInstantiateCategory(
        UpdateCategoryCommand input
    )
    {
        var dbContext = _fixture.CreateDbContext(NameDbContext);
        var exampleCategories = _fixture.ListCategoriesQueryFixture.GetExampleCategoriesList();
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<UpdateCategoryCommandHandler>();
        
        await dbContext.AddRangeAsync(exampleCategories);
        await dbContext.SaveChangesAsync();
        
        var repository = new CategoryRepository(dbContext);
        
        var command = new UpdateCategoryCommandHandler(repository, logger);
        input.Id = exampleCategories[0].Id;
    
        var task = async () => await command.Handle(input, CancellationToken.None);
    
        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage("Category is invalid");
    }
}
