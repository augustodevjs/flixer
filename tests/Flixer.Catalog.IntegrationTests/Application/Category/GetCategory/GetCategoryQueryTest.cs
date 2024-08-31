﻿using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Infra.Data.EF.Repositories;
using Flixer.Catalog.IntegrationTests.Fixtures.Repository;
using Flixer.Catalog.Application.Queries.Category.GetCategory;

namespace Flixer.Catalog.IntegrationTests.Application.Category.GetCategory;

[Collection(nameof(CategoryRepositoryFixture))]
public class GetCategoryQueryTest
{
    private readonly CategoryRepositoryFixture _fixture;
    private const string NameDbContext = "integration-tests-db";

    public GetCategoryQueryTest(CategoryRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("Integration/Application", "GetCategory - Query")]
    public async Task Query_GetCategory()
    {
        var exampleCategory = _fixture.CategoryFixture.GetValidCategory();
        var dbContext = _fixture.CreateDbContext(NameDbContext);
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<GetCategoryQueryHandler>();

        dbContext.Categories.Add(exampleCategory);
        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);

        var input = new GetCategoryQuery(exampleCategory.Id);
        var command = new GetCategoryQueryHandler(repository, logger);

        var output = await command.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(exampleCategory.Name);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        output.Description.Should().Be(exampleCategory.Description);
    }

    [Fact]
    [Trait("Integration/Application", "GetCategory - Query")]
    public async Task Query_NotFoundExceptionWhenCategoryDoesntExist()
    {
        var exampleCategory = _fixture.CategoryFixture.GetValidCategory();
        var dbContext = _fixture.CreateDbContext(NameDbContext);
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<GetCategoryQueryHandler>();
    
        dbContext.Categories.Add(exampleCategory);
        await dbContext.SaveChangesAsync();
    
        var repository = new CategoryRepository(dbContext);
        var input = new GetCategoryQuery(Guid.NewGuid());
    
        var command = new GetCategoryQueryHandler(repository, logger);
    
        var task = async () => await command.Handle(input, CancellationToken.None);
    
        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{input.Id}' not found.");
    }
}