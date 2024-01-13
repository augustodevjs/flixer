using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Exceptions;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.UseCases.Genre;
using Flixer.Catalog.Application.Dtos.InputModel.Genre;
using Flixer.Catalog.Application.Dtos.ViewModel.Genre;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Genre.UpdateGenreUseCase;

[Collection(nameof(UpdateGenreTestFixture))]
public class UpdateGenreUseCaseTest
{
    private readonly UpdateGenreTestFixture _fixture;

    public UpdateGenreUseCaseTest(UpdateGenreTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(UpdateGenre))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task UpdateGenre()
    {
        var exampleGenre = _fixture.GetExampleGenre();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var newNameExample = _fixture.GetValidGenreName();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();

        var newIsActive = !exampleGenre.IsActive;

        genreRepositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        var useCase = new UpdateGenre(
            unitOfWorkMock.Object,
            genreRepositoryMock.Object,
            _fixture.GetCategoryRepositoryMock().Object
        );

        var input = new UpdateGenreInputModel(
            exampleGenre.Id,
            newNameExample,
            newIsActive
        );

        GenreViewModel output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleGenre.Id);
        output.Categories.Should().HaveCount(0);
        output.Name.Should().Be(newNameExample);
        output.IsActive.Should().Be(newIsActive);
        output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);

        genreRepositoryMock.Verify(x => x.Update(
                It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(ThrowWhenNotFound))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task ThrowWhenNotFound()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();

        var exampleId = Guid.NewGuid();

        genreRepositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(new NotFoundException(
            $"Genre '{exampleId}' not found."
        ));

        var useCase = new UpdateGenre(
            _fixture.GetUnitOfWorkMock().Object,
            genreRepositoryMock.Object,
            _fixture.GetCategoryRepositoryMock().Object
        );

        var input = new UpdateGenreInputModel(exampleId, _fixture.GetValidGenreName(), true);

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre '{exampleId}' not found.");
    }

    [Theory(DisplayName = nameof(ThrowWhenNameIsInvalid))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task ThrowWhenNameIsInvalid(string? name)
    {

        var exampleGenre = _fixture.GetExampleGenre();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var newNameExample = _fixture.GetValidGenreName();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();

        var newIsActive = !exampleGenre.IsActive;
        genreRepositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        var useCase = new UpdateGenre(
            unitOfWorkMock.Object,
            genreRepositoryMock.Object,
            _fixture.GetCategoryRepositoryMock().Object
        );

        var input = new UpdateGenreInputModel(exampleGenre.Id, name!, newIsActive);

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<EntityValidationException>().WithMessage($"Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateGenreOnlyName))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateGenreOnlyName(bool isActive)
    {
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var newNameExample = _fixture.GetValidGenreName();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var exampleGenre = _fixture.GetExampleGenre(isActive: isActive);

        var newIsActive = !exampleGenre.IsActive;

        genreRepositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        var useCase = new UpdateGenre(
            unitOfWorkMock.Object,
            genreRepositoryMock.Object,
            _fixture.GetCategoryRepositoryMock().Object
        );

        var input = new UpdateGenreInputModel(exampleGenre.Id, newNameExample);

        GenreViewModel output =await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.IsActive.Should().Be(isActive);
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(newNameExample);
        output.Categories.Should().HaveCount(0);
        output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);

        genreRepositoryMock.Verify(
            x => x.Update(
                It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()),Times.Once);
    }

    [Fact(DisplayName = nameof(UpdateGenreAddingCategoriesIds))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task UpdateGenreAddingCategoriesIds()
    {
        var exampleGenre = _fixture.GetExampleGenre();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var newNameExample = _fixture.GetValidGenreName();
        var exampleCategoriesIdsList = _fixture.GetRandomIdsList();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();

        var newIsActive = !exampleGenre.IsActive;

        genreRepositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        categoryRepositoryMock.Setup(x => x.GetIdsListByIds(
            It.IsAny<List<Guid>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleCategoriesIdsList);

        var useCase = new UpdateGenre(
            unitOfWorkMock.Object,
            genreRepositoryMock.Object,
            categoryRepositoryMock.Object
        );

        var input = new UpdateGenreInputModel(
            exampleGenre.Id,
            newNameExample,
            newIsActive,
            exampleCategoriesIdsList
        );

        GenreViewModel output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(newNameExample);
        output.IsActive.Should().Be(newIsActive);
        output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
        output.Categories.Should().HaveCount(exampleCategoriesIdsList.Count);

        exampleCategoriesIdsList.ForEach(
            expectedId => output.Categories.Should().Contain(expectedId)
        );

        genreRepositoryMock.Verify(
            x => x.Update(
                It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()),Times.Once);
    }

    [Fact(DisplayName = nameof(UpdateGenreReplacingCategoriesIds))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task UpdateGenreReplacingCategoriesIds()
    {
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var newNameExample = _fixture.GetValidGenreName();
        var exampleCategoriesIdsList = _fixture.GetRandomIdsList();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var exampleGenre = _fixture.GetExampleGenre(categoriesIds: _fixture.GetRandomIdsList());

        var newIsActive = !exampleGenre.IsActive;

        genreRepositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        categoryRepositoryMock.Setup(x => x.GetIdsListByIds(
            It.IsAny<List<Guid>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleCategoriesIdsList);

        var useCase = new UpdateGenre(
            unitOfWorkMock.Object,
            genreRepositoryMock.Object,
            categoryRepositoryMock.Object
        );

        var input = new UpdateGenreInputModel(
            exampleGenre.Id,
            newNameExample,
            newIsActive,
            exampleCategoriesIdsList
        );

        GenreViewModel output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(newNameExample);
        output.IsActive.Should().Be(newIsActive);
        output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
        output.Categories.Should().HaveCount(exampleCategoriesIdsList.Count);

        exampleCategoriesIdsList.ForEach(
            expectedId => output.Categories.Should().Contain(expectedId)
        );

        genreRepositoryMock.Verify(
            x => x.Update(
                It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()),Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var exampleNewCategoriesIdsList = _fixture.GetRandomIdsList(10);
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var exampleGenre = _fixture.GetExampleGenre(categoriesIds: _fixture.GetRandomIdsList());

        var listReturnedByCategoryRepository = exampleNewCategoriesIdsList.GetRange(0, exampleNewCategoriesIdsList.Count - 2);
        var IdsNotReturnedByCategoryRepository = exampleNewCategoriesIdsList.GetRange(exampleNewCategoriesIdsList.Count - 2, 2);

        var newNameExample = _fixture.GetValidGenreName();

        var newIsActive = !exampleGenre.IsActive;

        genreRepositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        categoryRepositoryMock.Setup(x => x.GetIdsListByIds(
            It.IsAny<List<Guid>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(listReturnedByCategoryRepository);

        var useCase = new UpdateGenre(
            unitOfWorkMock.Object,
            genreRepositoryMock.Object,
            categoryRepositoryMock.Object
        );

        var input = new UpdateGenreInputModel(
            exampleGenre.Id,
            newNameExample,
            newIsActive,
            exampleNewCategoriesIdsList
        );

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        var notFoundIdsAsString = String.Join(", ", IdsNotReturnedByCategoryRepository);

        await action.Should().ThrowAsync<RelatedAggregateException>()
            .WithMessage(
            $"Related category id (or ids) not found: {notFoundIdsAsString}"
        );
    }

    [Fact(DisplayName = nameof(UpdateGenreWithoutCategoriesIds))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task UpdateGenreWithoutCategoriesIds()
    {
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var newNameExample = _fixture.GetValidGenreName();
        var exampleCategoriesIdsList = _fixture.GetRandomIdsList();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var exampleGenre = _fixture.GetExampleGenre(categoriesIds: exampleCategoriesIdsList);

        var newIsActive = !exampleGenre.IsActive;

        genreRepositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        var useCase = new UpdateGenre(
            unitOfWorkMock.Object,
            genreRepositoryMock.Object,
            categoryRepositoryMock.Object
        );

        var input = new UpdateGenreInputModel(
            exampleGenre.Id,
            newNameExample,
            newIsActive
        );

        GenreViewModel output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(newNameExample);
        output.IsActive.Should().Be(newIsActive);
        output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
        output.Categories.Should().HaveCount(exampleCategoriesIdsList.Count);

        exampleCategoriesIdsList.ForEach(
            expectedId => output.Categories.Should().Contain(expectedId)
        );

        genreRepositoryMock.Verify(
            x => x.Update(
                It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()),Times.Once);
    }

    [Fact(DisplayName = nameof(UpdateGenreWithEmptyCategoriesIdsList))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task UpdateGenreWithEmptyCategoriesIdsList()
    {
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var newNameExample = _fixture.GetValidGenreName();
        var exampleCategoriesIdsList = _fixture.GetRandomIdsList();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var exampleGenre = _fixture.GetExampleGenre(categoriesIds: exampleCategoriesIdsList);

        var newIsActive = !exampleGenre.IsActive;

        genreRepositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        var useCase = new UpdateGenre(
            unitOfWorkMock.Object,
            genreRepositoryMock.Object,
            categoryRepositoryMock.Object
        );
        var input = new UpdateGenreInputModel(
            exampleGenre.Id,
            newNameExample,
            newIsActive,
            new List<Guid>()
        );

        GenreViewModel output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(newNameExample);
        output.Categories.Should().HaveCount(0);
        output.IsActive.Should().Be(newIsActive);
        output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);

        genreRepositoryMock.Verify(
            x => x.Update(
                It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()),Times.Once);
    }
}
