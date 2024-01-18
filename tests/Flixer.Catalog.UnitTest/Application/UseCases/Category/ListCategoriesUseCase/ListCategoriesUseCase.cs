using Flixer.Catalog.Application.Dtos.InputModel.Category;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;
using Flixer.Catalog.Application.UseCases.Category;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;
using DomainEntity = Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.ListCategoriesUseCase;

[Collection(nameof(ListCategoriesUseCaseTestFixture))]
public class ListCategoriesUseCase
{
    private readonly ListCategoriesUseCaseTestFixture _fixture;

    public ListCategoriesUseCase(ListCategoriesUseCaseTestFixture fixture)
        => _fixture = fixture;

    [Fact]
    [Trait("Application", "ListCategories - Use Cases")]
    public async Task UseCase_ShouldListCategories_WhenMethodHandleIsCalled()
    {
        var input = _fixture.GetExampleInput();
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
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new ListCategories(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

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

        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }


    [Fact]
    [Trait("Application", "ListCategories - Use Cases")]
    public async Task UseCase_ShouldNotListCategories_WhenDoesntHaveCategories()
    {
        var input = _fixture.GetExampleInput();
        var repositoryMock = _fixture.GetRepositoryMock();

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
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new ListCategories(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Total.Should().Be(0);
        output.Should().NotBeNull();
        output.Items.Should().HaveCount(0);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);

        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Theory]
    [Trait("Application", "ListCategories - Use Cases")]
    [MemberData(
        nameof(ListCategoriesUseCaseTestDataGenerator.GetInputsWithoutAllParameter),
        parameters: 14,
        MemberType = typeof(ListCategoriesUseCaseTestDataGenerator)
    )]
    public async Task UseCase_ShouldListCategories_WhenParametersAreNotProvided(
        ListCategoriesInputModel input
    )
    {
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
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new ListCategories(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

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
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
