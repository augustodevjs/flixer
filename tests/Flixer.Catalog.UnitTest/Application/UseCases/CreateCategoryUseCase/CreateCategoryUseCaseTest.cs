using Flixer.Catalog.Domain.Entities;
using UseCase = Flixer.Catalog.Application.UseCases.Category.CreateCategory;

namespace Flixer.Catalog.UnitTest.Application.UseCases.CreateCategoryUseCase;

[Collection(nameof(CreateCategoryUseCaseTestFixture))]
public class CreateCategoryUseCaseTest
{
    private readonly CreateCategoryUseCaseTestFixture _fixture;

    public CreateCategoryUseCaseTest(CreateCategoryUseCaseTestFixture fixture) =>
        _fixture = fixture;

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        var unitOfWorkMock = _fixture.GetUnityOfWorkMock();
        var repositoryMock = _fixture.GetCategoryRepositoryMock();

        var useCase = new UseCase.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().NotBeSameDateAs(default);

        unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.Insert(
            It.IsAny<Category>(),
            It.IsAny<CancellationToken>()), 
            Times.Once
        );
    }
}