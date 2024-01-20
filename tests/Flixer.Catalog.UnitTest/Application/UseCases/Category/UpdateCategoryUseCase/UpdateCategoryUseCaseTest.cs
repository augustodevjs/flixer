using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Common.Tests.Fixture.Category;
using Flixer.Catalog.Application.UseCases.Category;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;
using Flixer.Catalog.Application.Dtos.InputModel.Category;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.UpdateCategoryUseCase;

[Collection(nameof(CategoryTestFixture))]
public class UpdateCategoryUseCaseTest
{
    private readonly CategoryTestFixture _fixture;

    public UpdateCategoryUseCaseTest(CategoryTestFixture fixture)
        => _fixture = fixture;

    [Theory]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(DataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(DataGenerator)
    )]
    public async Task UseCase_ShouldUpdateCategory_WhenMethodHandleIsCalled(
        DomainEntity.Category exampleCategory, 
        UpdateCategoryInputModel input
    )
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

    [Theory]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(DataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(DataGenerator)
    )]
    public async Task UseCase_ShouldUpdateCategory_WhenMethodHandleIsCalledWithoutProvidingIsActive(
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


    [Theory]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(DataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(DataGenerator)
    )]
    public async Task UseCase_ShouldUpdateCategory_WhenMethodHandleIsCalledWithOnlyName(
        DomainEntity.Category exampleCategory, 
        UpdateCategoryInputModel exampleInput
    )
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

    [Fact]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task UseCase_ShouldThrowError_WhenCategoryNotFound()
    {
        var input = _fixture.GetInputUpdate();
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

    [Theory]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(DataGenerator.GetInvalidUpdateInputs),
        parameters: 12,
        MemberType = typeof(DataGenerator)
    )]
    public async Task UseCase_ShouldThrowError_WhenCantUpdateCategory(
        UpdateCategoryInputModel input, 
        string expectedExceptionMessage
    )
    {
        var exampleCategory = _fixture.GetValidCategory();
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
