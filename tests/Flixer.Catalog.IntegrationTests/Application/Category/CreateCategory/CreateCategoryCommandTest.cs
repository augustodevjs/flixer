using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Infra.Data.EF.Repositories;
using Flixer.Catalog.IntegrationTests.Fixtures.Repository;
using Flixer.Catalog.Application.Commands.Category.CreateCategory;

namespace Flixer.Catalog.IntegrationTests.Application.Category.CreateCategory;

[Collection(nameof(CategoryRepositoryFixture))]
public class CreateCategoryCommandTest
{
    private readonly CategoryRepositoryFixture _fixture;
    private const string NameDbContext = "integration-tests-db";

    public CreateCategoryCommandTest(CategoryRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("Integration/Application", "CreateCategory - Command")]
    public async void Command_CreateCategory()
    {
        var input = _fixture.CreateCategoryCommandFixture.GetInputCreate();
        var dbContext = _fixture.CreateDbContext(NameDbContext);
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<CreateCategoryCommandHandler>();

        var repository = new CategoryRepository(dbContext);

        var command = new CreateCategoryCommandHandler(repository, logger);

        var output = await command.Handle(input, CancellationToken.None);

        var dbCategory = await _fixture.CreateDbContext(NameDbContext, true)
            .Categories.FindAsync(output.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        dbCategory.Description.Should().Be(input.Description);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }
    
    [Fact]
    [Trait("Integration/Application", "CreateCategory - Command")]
    public async void Command_CreateCategoryOnlyWithNameAndDescription()
    {
        var exampleInput = _fixture.CreateCategoryCommandFixture.GetInputCreate();
        var dbContext = _fixture.CreateDbContext(NameDbContext);
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<CreateCategoryCommandHandler>();
    
        var repository = new CategoryRepository(dbContext);
    
        var command = new CreateCategoryCommandHandler(repository, logger);
    
        var input = new CreateCategoryCommand(exampleInput.Name, exampleInput.Description);
    
        var output = await command.Handle(input, CancellationToken.None);
    
        var dbCategory = await _fixture.CreateDbContext(NameDbContext, true)
            .Categories.FindAsync(output.Id);
    
        dbCategory.Should().NotBeNull();
        dbCategory!.IsActive.Should().Be(true);
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        dbCategory.Description.Should().Be(input.Description);
    
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.IsActive.Should().Be(true);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }
}
