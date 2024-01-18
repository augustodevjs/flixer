using Flixer.Catalog.Application.Dtos.InputModel.Genre;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.UseCases.Genre;
using DomainEntity = Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Genre.DeleteGenreUseCase;

[Collection(nameof(DeleteGenreTestFixture))]
public class DeleteGenreUseCaseTest
{
    private readonly DeleteGenreTestFixture _fixture;

    public DeleteGenreUseCaseTest(DeleteGenreTestFixture fixture)
        => _fixture = fixture;

    [Fact]
    [Trait("Application", "DeleteGenre - Use Cases")]
    public async Task UseCase_DeleteGenre_WhenMethodHandleIsCalled()
    {
        var exampleGenre = _fixture.GetExampleGenre();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();

        genreRepositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        var useCase = new DeleteGenre(unitOfWorkMock.Object, genreRepositoryMock.Object);

        var input = new DeleteGenreInputModel(exampleGenre.Id);

        await useCase.Handle(input, CancellationToken.None);

        genreRepositoryMock.Verify(x => x.Get(
                It.Is<Guid>(x => x == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        genreRepositoryMock.Verify(x => x.Delete(
                It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait("Application", "DeleteGenre - Use Cases")]
    public async Task UseCase_ShouldThrowError_WhenGenreIsNotFound()
    {
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var exampleId = Guid.NewGuid();

        var useCase = new DeleteGenre(unitOfWorkMock.Object, genreRepositoryMock.Object);

        var input = new DeleteGenreInputModel(exampleId);

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre '{exampleId}' not found.");

        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);

        genreRepositoryMock.Verify(x => x.Get(
                It.Is<Guid>(x => x == exampleId),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}