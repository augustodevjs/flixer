using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Commands.Category.CreateCategory;
using Flixer.Catalog.UnitTest.Application.Fixtures.Category.CreateCategory;
using Flixer.Catalog.UnitTest.Helpers;

namespace Flixer.Catalog.UnitTest.Application.Category;

[Collection(nameof(CreateCategoryCommandFixtureCollection))]
public class CreateCategoryCommandTest
{
    private readonly CreateCategoryCommandFixture _fixture;

    public CreateCategoryCommandTest(CreateCategoryCommandFixture fixture) =>
        _fixture = fixture;

    [Fact]
    [Trait("Application", "CreateCategory - Command")]
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
        
        repositoryMock.Verify(repository => repository.Create(
            It.IsAny<Catalog.Domain.Entities.Category>()), Times.Once);
        
        repositoryMock.Verify(repository => 
            repository.UnityOfWork.Commit(), Times.Once);
        
        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(3));
    }

    [Fact]
    [Trait("Application", "CreateCategory - Command")]
    public async void Command_ShouldCreateCategory_WhenMethodHandleIsCalledWithOnlyNameAndDescription()
    {
        var loggerMock = _fixture.GetLoggerMock();
        var repositoryMock = _fixture.GetRepositoryMock();

        var command = new CreateCategoryCommandHandler(repositoryMock.Object, loggerMock.Object);

        var input = _fixture.GetInputCreateWithNameAndDescription();
        
        repositoryMock.Setup(repo => repo.UnityOfWork.Commit())
            .ReturnsAsync(true);

        var output = await command.Handle(input, CancellationToken.None);
    
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().BeTrue();
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().NotBeSameDateAs(default);
    
        repositoryMock.Verify(repository => repository.Create(
            It.IsAny<Catalog.Domain.Entities.Category>()), Times.Once);
        
        repositoryMock.Verify(repository => 
            repository.UnityOfWork.Commit(), Times.Once);
        
        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(3));
    }
    
    [Theory]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(
        nameof(DataGenerator.GetInvalidCreateInputs),
        parameters: 24,
        MemberType = typeof(DataGenerator)
     )]
    public async void UseCase_ShouldThrowError_WhenMethodHandleIsCalledWithInvalidInputs(CreateCategoryCommand input)
    {
        var loggerMock = _fixture.GetLoggerMock();
        var repositoryMock = _fixture.GetRepositoryMock();

        var command = new CreateCategoryCommandHandler(repositoryMock.Object, loggerMock.Object);
    
        Func<Task> task = async () => await command.Handle(input, CancellationToken.None);
    
        await task.Should().ThrowAsync<EntityValidationException>().WithMessage("Category is invalid");
        
        loggerMock.VerifyLog(LogLevel.Error, Times.Exactly(1));
        repositoryMock.Verify(uow => uow.UnityOfWork.Commit(), Times.Never);
        repositoryMock.Verify(repository => repository.Create(
            It.IsAny<Catalog.Domain.Entities.Category>()),
            Times.Never
        );
    }
}