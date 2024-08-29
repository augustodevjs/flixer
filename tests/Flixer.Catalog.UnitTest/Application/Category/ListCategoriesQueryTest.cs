using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;
using Flixer.Catalog.Application.Queries.Category.ListCategories;
using Flixer.Catalog.Application.Commands.Category.CreateCategory;
using Flixer.Catalog.UnitTest.Fixture.Application.Category.ListCategory;
using Flixer.Catalog.UnitTest.Helpers;

namespace Flixer.Catalog.UnitTest.Application.Category;

[Collection(nameof(ListCategoriesQueryFixture))]
public class ListCategoriesQueryTest
{
    private readonly ListCategoriesQueryFixture _fixture;

    public ListCategoriesQueryTest(ListCategoriesQueryFixture fixture)
        => _fixture = fixture;

    [Fact]
    [Trait("Application", "ListCategories - Query")]
    public async Task Query_ShouldListCategories_WhenMethodHandleIsCalled()
    {
        var loggerMock = _fixture.GetLogger();
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = _fixture.GetListInput();
        var categoriesExampleList = _fixture.GetExampleCategoriesList();

        var outputRepositorySearch = new SearchOutput<DomainEntity.Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: categoriesExampleList,
            total: new Random().Next(50, 200)
        );

        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            )
        )).ReturnsAsync(outputRepositorySearch);

        var command = new ListCategoriesQueryHandler(repositoryMock.Object, loggerMock.Object);

        var output = await command.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);

        ((List<CategoryViewModel>)output.Items).ForEach(outputItem =>
        {
            var repositoryCategory = outputRepositorySearch.Items
                .FirstOrDefault(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryCategory!.Name);
            outputItem.Description.Should().Be(repositoryCategory!.Description);
            outputItem.IsActive.Should().Be(repositoryCategory!.IsActive);
            outputItem.CreatedAt.Should().Be(repositoryCategory!.CreatedAt);
        });
        
        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(2));

        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            )
        ), Times.Once);
    }

    [Fact]
    [Trait("Application", "ListCategories - Query")]
    public async Task Query_ShouldNotListCategories_WhenDoesntHaveCategories()
    {
        var loggerMock = _fixture.GetLogger();
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = _fixture.GetListInput();
    
        var outputRepositorySearch = new SearchOutput<DomainEntity.Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: new List<DomainEntity.Category>().AsReadOnly(),
            total: 0
        );
    
        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            )
        )).ReturnsAsync(outputRepositorySearch);
    
        var command = new ListCategoriesQueryHandler(repositoryMock.Object, loggerMock.Object);
    
        var output = await command.Handle(input, CancellationToken.None);
    
        output.Total.Should().Be(0);
        output.Should().NotBeNull();
        output.Items.Should().HaveCount(0);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        
        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(2));

        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            )
        ), Times.Once);
    }
    
    [Theory]
    [Trait("Application", "ListCategories - Command")]
    [MemberData(
        nameof(DataGenerator.GetInputsListWithoutAllParameter),
        parameters: 14,
        MemberType = typeof(DataGenerator)
    )]
    public async Task Command_ShouldListCategories_WhenParametersAreNotProvided(
        ListCategoriesQuery input
    )
    {
        var loggerMock = _fixture.GetLogger();
        var repositoryMock = _fixture.GetRepositoryMock();
        var categoriesExampleList = _fixture.GetExampleCategoriesList();
    
        var outputRepositorySearch = new SearchOutput<DomainEntity.Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: categoriesExampleList,
            total: new Random().Next(50, 200)
        );
    
        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            )
        )).ReturnsAsync(outputRepositorySearch);
    
        var command = new ListCategoriesQueryHandler(repositoryMock.Object, loggerMock.Object);
    
        var output = await command.Handle(input, CancellationToken.None);
    
        output.Should().NotBeNull();
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
        ((List<CategoryViewModel>)output.Items).ForEach(outputItem =>
        {
            var repositoryCategory = outputRepositorySearch.Items
                .FirstOrDefault(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryCategory!.Name);
            outputItem.Description.Should().Be(repositoryCategory!.Description);
            outputItem.IsActive.Should().Be(repositoryCategory!.IsActive);
            outputItem.CreatedAt.Should().Be(repositoryCategory!.CreatedAt);
        });
        
        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(2));
        
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            )
        ), Times.Once);
    }
}
