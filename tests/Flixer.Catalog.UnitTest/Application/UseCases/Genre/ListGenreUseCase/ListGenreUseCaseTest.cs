using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.UseCases.Genre.Common;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;
using Flixer.Catalog.Application.UseCases.Genre.ListGenres;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Genre.ListGenreUseCase;

[Collection(nameof(ListGenresTestFixture))]
public class ListGenreUseCaseTest
{
    private readonly ListGenresTestFixture _fixture;

    public ListGenreUseCaseTest(ListGenresTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ListGenres))]
    [Trait("Application", "ListGenres - Use Cases")]
    public async Task ListGenres()
    {
        var genresListExample = _fixture.GetExampleGenresList();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();

        var input = _fixture.GetExampleInput();
        var outputRepositorySearch = new SearchOutput<DomainEntity.Genre>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: genresListExample,
            total: new Random().Next(50, 200)
        );

        genreRepositoryMock.Setup(x => x.Search(
            It.IsAny<SearchInput>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new ListGenres(genreRepositoryMock.Object);

        ListGenresOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);

        ((List<GenreModelOutput>)output.Items).ForEach(outputItem =>
        {
            var repositoryGenre = outputRepositorySearch.Items.FirstOrDefault(x => x.Id == outputItem.Id);

            outputItem.Should().NotBeNull();
            repositoryGenre.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryGenre!.Name);
            outputItem.IsActive.Should().Be(repositoryGenre.IsActive);
            outputItem.CreatedAt.Should().Be(repositoryGenre!.CreatedAt);
            outputItem.Categories.Should()
                .HaveCount(repositoryGenre.Categories.Count);
            foreach (var expectedId in repositoryGenre.Categories)
                outputItem.Categories.Should().Contain(expectedId);
        });

        genreRepositoryMock.Verify(
            x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(ListEmpty))]
    [Trait("Application", "ListGenres - Use Cases")]
    public async Task ListEmpty()
    {
        var input = _fixture.GetExampleInput();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();

        var outputRepositorySearch = new SearchOutput<DomainEntity.Genre>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: new List<DomainEntity.Genre>(),
            total: new Random().Next(50, 200)
        );

        genreRepositoryMock.Setup(x => x.Search(
            It.IsAny<SearchInput>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new ListGenres(genreRepositoryMock.Object);

        ListGenresOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Total.Should().Be(outputRepositorySearch.Total);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);

        genreRepositoryMock.Verify(
            x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(ListUsingDefaultInputValues))]
    [Trait("Application", "ListGenres - Use Cases")]
    public async Task ListUsingDefaultInputValues()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();

        var outputRepositorySearch = new SearchOutput<DomainEntity.Genre>(
            currentPage: 1,
            perPage: 15,
            items: new List<DomainEntity.Genre>(),
            total: 0
        );

        genreRepositoryMock.Setup(x => x.Search(
            It.IsAny<SearchInput>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new ListGenres(genreRepositoryMock.Object);

        ListGenresOutput output = await useCase.Handle(new ListGenresInput(), CancellationToken.None);

        genreRepositoryMock.Verify(
            x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == 1
                    && searchInput.PerPage == 15
                    && searchInput.Search == ""
                    && searchInput.OrderBy == ""
                    && searchInput.Order == SearchOrder.Asc
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}