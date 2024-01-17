using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.UseCases.Genre;
using Flixer.Catalog.Application.Dtos.ViewModel.Genre;
using Flixer.Catalog.Application.Dtos.InputModel.Genre;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Genre.GetGenreUseCase;

[Collection(nameof(GetGenreTestFixture))]
public class GetGenreUseCaseTest
{
    private readonly GetGenreTestFixture _fixture;

    public GetGenreUseCaseTest(GetGenreTestFixture fixture)
        => _fixture = fixture;

    [Fact]
    [Trait("Application", "GetGenre - Use Cases")]
    public async Task UseCase_ShouldGetGenre_WhenMethodIsCalled()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var exampleGenre = _fixture.GetExampleGenre(categoriesIds: _fixture.GetRandomIdsList());

        genreRepositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        var useCase = new GetGenre(genreRepositoryMock.Object);
        var input = new GetGenreInputModel(exampleGenre.Id);

        GenreViewModel output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(exampleGenre.Name);
        output.IsActive.Should().Be(exampleGenre.IsActive);
        output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
        output.Categories.Should().HaveCount(exampleGenre.Categories.Count);

        foreach (var expectedId in exampleGenre.Categories)
        {
            output.Categories.Should().Contain(expectedId);
        }

        genreRepositoryMock.Verify(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id), 
            It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    [Trait("Application", "GetGenre - Use Cases")]
    public async Task UseCase_ShouldThrowError_WhenGenreIsNotFound()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var exampleId = Guid.NewGuid();

        var useCase = new GetGenre(genreRepositoryMock.Object);
        var input = new GetGenreInputModel(exampleId);

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre '{exampleId}' not found.");

        genreRepositoryMock.Verify(
            x => x.Get(
                It.Is<Guid>(x => x == exampleId),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}
