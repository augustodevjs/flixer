using Flixer.Catalog.UnitTest.Application.Fixtures.Category;
using Flixer.Catalog.Application.Commands.Category.CreateCategory;

namespace Flixer.Catalog.UnitTest.Application.Category;

[Collection(nameof(CreateCategoryCommandFixtureCollection))]
public class CreateCategoryCommandTest
{
    private readonly CreateCategoryCommandFixture _fixture;

    public CreateCategoryCommandTest(CreateCategoryCommandFixture fixture) =>
        _fixture = fixture;

    [Fact]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void Command_ShouldCreateCategory_WhenMethodHandleIsCalled()
    {
        var loggerMock = _fixture.GetLoggerMock();
        var repositoryMock = _fixture.GetRepositoryMock();

        var command = new CreateCategoryCommandHandler(repositoryMock.Object, loggerMock.Object);

        var input = _fixture.GetInputCreate();
        
        repositoryMock.Setup(repo => repo.UnityOfWork.Commit())
            .ReturnsAsync(true);

        var output = await command.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().NotBeSameDateAs(default);

        // repositoryMock.Verify(repository => repository.Create(
        //     It.IsAny<DomainEntity.Category>(),
        //     Times.Once
        // );
    }

    // [Fact]
    // [Trait("Application", "CreateCategory - Use Cases")]
    // public async void UseCase_ShouldCreateCategory_WhenMethodHandleIsCalledWithOnlyName()
    // {
    //     var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
    //     var repositoryMock = _fixture.GetRepositoryMock();
    //
    //     var useCase = new UseCase.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);
    //
    //     var input = new CreateCategoryInputModel(_fixture.GetValidCategoryName());
    //
    //     var output = await useCase.Handle(input, CancellationToken.None);
    //
    //     output.Should().NotBeNull();
    //     output.Id.Should().NotBeEmpty();
    //     output.Name.Should().Be(input.Name);
    //     output.IsActive.Should().BeTrue();
    //     output.Description.Should().Be("");
    //     output.CreatedAt.Should().NotBeSameDateAs(default);
    //
    //     unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
    //     repositoryMock.Verify(repository => repository.Insert(
    //         It.IsAny<DomainEntity.Category>(),
    //         It.IsAny<CancellationToken>()),
    //         Times.Once
    //     );
    // }
    //
    // [Fact]
    // [Trait("Application", "CreateCategory - Use Cases")]
    // public async void UseCase_ShouldCreateCategory_WhenMethodHandleIsCalledWithOnlyNameAndDescription()
    // {
    //     var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
    //     var repositoryMock = _fixture.GetRepositoryMock();
    //
    //     var useCase = new UseCase.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);
    //
    //     var input = new CreateCategoryInputModel(_fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());
    //
    //     var output = await useCase.Handle(input, CancellationToken.None);
    //
    //     output.Should().NotBeNull();
    //     output.Id.Should().NotBeEmpty();
    //     output.Name.Should().Be(input.Name);
    //     output.IsActive.Should().BeTrue();
    //     output.Description.Should().Be(input.Description);
    //     output.CreatedAt.Should().NotBeSameDateAs(default);
    //
    //     unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
    //     repositoryMock.Verify(repository => repository.Insert(
    //         It.IsAny<DomainEntity.Category>(),
    //         It.IsAny<CancellationToken>()),
    //         Times.Once
    //     );
    // }
    //
    // [Theory]
    // [Trait("Application", "CreateCategory - Use Cases")]
    // [MemberData(
    //     nameof(DataGenerator.GetInvalidCreateInputs),
    //     parameters: 24,
    //     MemberType = typeof(DataGenerator)
    //  )]
    // public async void UseCase_ShouldThrowError_WhenMethodHandleIsCalledWithInvalidInputs(
    //     CreateCategoryInputModel input,
    //     string exceptionMessage
    // )
    // {
    //     var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
    //     var repositoryMock = _fixture.GetRepositoryMock();
    //
    //     var useCase = new UseCase.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);
    //
    //     Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);
    //
    //     await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
    //     unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Never);
    //     repositoryMock.Verify(repository => repository.Insert(
    //         It.IsAny<DomainEntity.Category>(),
    //         It.IsAny<CancellationToken>()),
    //         Times.Never
    //     );
    // }
}