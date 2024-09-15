using Xunit;
using FluentAssertions;
using Flixer.Catalog.Domain.Enums;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Infra.Data.EF.Repositories;
using Flixer.Catalog.Application.Common.Input.Category;
using Flixer.Catalog.IntegrationTests.Fixtures.Repository;

namespace Flixer.Catalog.IntegrationTests.Application.Category;

[Collection(nameof(CategoryRepositoryFixture))]
public class ListCategoriesTest
{
    private readonly CategoryRepositoryFixture _fixture;
    private const string NameDbContext = "integration-tests-db";

    public ListCategoriesTest(CategoryRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("Integration/Application", "ListCategories - Query")]
    public async Task Query_SearchReturnsListAndTotal()
    {
        var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList();
        var dbContext = _fixture.CreateDbContext(NameDbContext);
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<Catalog.Application.Queries.Category.ListCategories>();

        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new CategoryRepository(dbContext);

        var input = new ListCategoriesInput(1, 20);

        var command = new Catalog.Application.Queries.Category.ListCategories(logger, categoryRepository);

        var output = await command.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().HaveCount(exampleCategoriesList.Count);

        foreach (var outputItem in output.Items)
        {
            var exampleItem = exampleCategoriesList.Find(
                category => category.Id == outputItem.Id
            );
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }

    [Fact]
    [Trait("Integration/Application", "ListCategories - Query")]
    public async Task Query_SearchReturnsEmptyWhenEmpty()
    {
        var dbContext = _fixture.CreateDbContext(NameDbContext);
        var categoryRepository = new CategoryRepository(dbContext);
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<Catalog.Application.Queries.Category.ListCategories>();
    
        var input = new ListCategoriesInput(1, 20);
        var command = new Catalog.Application.Queries.Category.ListCategories(logger, categoryRepository);
    
        var output = await command.Handle(input, CancellationToken.None);
    
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }
    
    [Theory]
    [Trait("Integration/Application", "ListCategories - Query")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task Query_SearchRetursPaginated(
        int quantityCategoriesToGenerate,
        int page,
        int perPage,
        int expectedQuantityItems
    )
    {
        var dbContext = _fixture.CreateDbContext(NameDbContext);
        var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList(
            quantityCategoriesToGenerate
        );
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<Catalog.Application.Queries.Category.ListCategories>();
    
        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
    
        var input = new ListCategoriesInput(page, perPage);
        var categoryRepository = new CategoryRepository(dbContext);
    
        var command = new Catalog.Application.Queries.Category.ListCategories(logger, categoryRepository);
    
        var output = await command.Handle(input, CancellationToken.None);
    
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().HaveCount(expectedQuantityItems);
    
        foreach (var outputItem in output.Items)
        {
            var exampleItem = exampleCategoriesList.Find(
                category => category.Id == outputItem.Id
            );
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }
    
    [Theory]
    [Trait("Integration/Application", "ListCategories - Query")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task Query_SearchByText(
        string search,
        int page,
        int perPage,
        int expectedQuantityItemsReturned,
        int expectedQuantityTotalItems
    )
    {
        var categoryNamesList = new List<string>() {
            "Action",
            "Horror",
            "Horror - Robots",
            "Horror - Based on Real Facts",
            "Drama",
            "Sci-fi IA",
            "Sci-fi Space",
            "Sci-fi Robots",
            "Sci-fi Future"
        };
    
        var dbContext = _fixture.CreateDbContext(NameDbContext);
        var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesListWithNames(
            categoryNamesList
        );
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<Catalog.Application.Queries.Category.ListCategories>();
    
        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        var categoryRepository = new CategoryRepository(dbContext);
    
        var input = new ListCategoriesInput(page, perPage, search);
        var command = new Catalog.Application.Queries.Category.ListCategories(logger, categoryRepository);
    
        var output = await command.Handle(input, CancellationToken.None);
    
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQuantityTotalItems);
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);
    
        foreach (var outputItem in output.Items)
        {
            var exampleItem = exampleCategoriesList.Find(
                category => category.Id == outputItem.Id
            );
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }
    
    [Theory]
    [Trait("Integration/Application", "ListCategories - Query")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    [InlineData("", "asc")]
    public async Task Query_SearchOrdered(
        string orderBy,
        string order
    )
    {
        var exampleCategoriesList = _fixture.DataGenerator.GetExampleCategoriesList();
        var dbContext = _fixture.CreateDbContext(NameDbContext);
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        var logger = loggerFactory.CreateLogger<Catalog.Application.Queries.Category.ListCategories>();
    
        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        var categoryRepository = new CategoryRepository(dbContext);
    
        var commandOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var input = new ListCategoriesInput(1, 20, "", orderBy, commandOrder);
        
        var useCase = new Catalog.Application.Queries.Category.ListCategories(logger, categoryRepository);
    
        var output = await useCase.Handle(input, CancellationToken.None);
    
        var expectedOrderedList = _fixture.DataGenerator.CloneCategoriesListOrdered(
            exampleCategoriesList,
            input.Sort,
            input.Dir
        );
    
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().HaveCount(exampleCategoriesList.Count);
    
        for (var index = 0; index < expectedOrderedList.Count; index++)
        {
            var outputItem = output.Items[index];
            var exampleItem = expectedOrderedList[index];
            outputItem.Should().NotBeNull();
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Id.Should().Be(exampleItem.Id);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }
}
