using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.UseCases.Category;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;
using Flixer.Catalog.Application.Dtos.InputModel.Category;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.UpdateCategoryUseCase;

[Collection(nameof(UpdateCategoryUseCaseTestFixture))]
public class UpdateCategoryUseCaseTest
{
    private readonly UpdateCategoryUseCaseTestFixture _fixture;

    public UpdateCategoryUseCaseTest(UpdateCategoryUseCaseTestFixture fixture)
        => _fixture = fixture;

    [Theory(DisplayName = nameof(UpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryUseCaseTestDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryUseCaseTestDataGenerator)
    )]
    public async Task UpdateCategory(DomainEntity.Category exampleCategory, UpdateCategoryInputModel input)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(
            exampleCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategory);

        var useCase = new UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        CategoryViewModel output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(exampleCategory, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingIsActive))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryUseCaseTestDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryUseCaseTestDataGenerator)
    )]
    public async Task UpdateCategoryWithoutProvidingIsActive(
        DomainEntity.Category exampleCategory,
        UpdateCategoryInputModel exampleInput
    )
    {
        var input = new UpdateCategoryInputModel(
            exampleInput.Id,
            exampleInput.Name,
            null,
            exampleInput.Description
        );

        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(
            exampleCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategory);

        var useCase = new UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        CategoryViewModel output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(exampleCategory, It.IsAny<CancellationToken>()), Times.Once);
    }


    [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryUseCaseTestDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryUseCaseTestDataGenerator)
    )]
    public async Task UpdateCategoryOnlyName(DomainEntity.Category exampleCategory, UpdateCategoryInputModel exampleInput)
    {
        var input = new UpdateCategoryInputModel(
            exampleInput.Id,
            exampleInput.Name
        );

        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(
            exampleCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategory);

        var useCase = new UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        CategoryViewModel output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.Description.Should().Be(exampleCategory.Description);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(exampleCategory, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        var input = _fixture.GetValidInput();
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{input.Id}' not found.");

        repositoryMock.Verify(x => x.Get(
            input.Id,
            It.IsAny<CancellationToken>())
        , Times.Once);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantUpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryUseCaseTestDataGenerator.GetInvalidInputs),
        parameters: 12,
        MemberType = typeof(UpdateCategoryUseCaseTestDataGenerator)
    )]
    public async Task ThrowWhenCantUpdateCategory(UpdateCategoryInputModel input, string expectedExceptionMessage)
    {
        var exampleCategory = _fixture.GetExampleCategory();
        input.Id = exampleCategory.Id;

        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(
            exampleCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategory);

        var useCase = new UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(expectedExceptionMessage);

        repositoryMock.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
