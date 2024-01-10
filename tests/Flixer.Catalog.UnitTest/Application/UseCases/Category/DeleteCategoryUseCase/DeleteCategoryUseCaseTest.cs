using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.UseCases.Category.DeleteCategory;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.DeleteCategoryUseCase;

[Collection(nameof(DeleteCategoryUseCaseTestFixture))]
public class DeleteCategoryUseCaseTest
{
    private readonly DeleteCategoryUseCaseTestFixture _fixture;

    public DeleteCategoryUseCaseTest(DeleteCategoryUseCaseTestFixture fixture) =>
        _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var categoryExample = _fixture.GetExampleCategory();

        repositoryMock.Setup(x => x.Get(
            categoryExample.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(categoryExample);

        var input = new DeleteCategoryInput(categoryExample.Id);

        var useCase = new DeleteCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
         );

        await useCase.Handle(input, CancellationToken.None);

        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Delete(categoryExample, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        var exampleGuid = Guid.NewGuid();
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(
            exampleGuid,
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(
            new NotFoundException($"Category '{exampleGuid}' not found")
        );

        var input = new DeleteCategoryInput(exampleGuid);

        var useCase = new DeleteCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
        repositoryMock.Verify(x => x.Get(exampleGuid, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Delete(It.IsAny<DomainEntity.Category>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}