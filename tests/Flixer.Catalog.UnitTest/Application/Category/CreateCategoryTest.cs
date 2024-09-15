using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Tests.Shared.Helpers;
using Flixer.Catalog.Application.Commands.Category;
using Flixer.Catalog.Application.Common.Input.Category;
using Flixer.Catalog.UnitTest.Fixture.Application.Category.CreateCategory;

namespace Flixer.Catalog.UnitTest.Application.Category;

[Collection(nameof(CreateCategoryFixture))]
public class CreateCategoryTest
{
    private readonly CreateCategoryFixture _fixture;

    public CreateCategoryTest(CreateCategoryFixture fixture) =>
        _fixture = fixture;

    [Fact]
    [Trait("Application", "CreateCategory - Command")]
    public async void Command_ShouldCreateCategory_WhenMethodHandleIsCalled()
    {
        var loggerMock = _fixture.GetLoggerMock();
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var command = new CreateCategory(unitOfWorkMock.Object, loggerMock.Object, repositoryMock.Object);

        var input = _fixture.DataGenerator.GetInputCreate();
        
        unitOfWorkMock.Setup(uow => uow.Commit())
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
        
        unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
        
        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(1));
    }

    [Fact]
    [Trait("Application", "CreateCategory - Command")]
    public async void Command_ShouldCreateCategory_WhenMethodHandleIsCalledWithOnlyNameAndDescription()
    {
        var loggerMock = _fixture.GetLoggerMock();
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var command = new CreateCategory(unitOfWorkMock.Object, loggerMock.Object, repositoryMock.Object);

        var input = _fixture.DataGenerator.GetInputCreateWithNameAndDescription();
        
        unitOfWorkMock.Setup(uow => uow.Commit())
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
        
        unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
        
        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(1));
    }
    
    [Theory]
    [Trait("Application", "CreateCategory - Command")]
    [MemberData(
        nameof(DataGenerator.GetInvalidCreateInputs),
        parameters: 24,
        MemberType = typeof(DataGenerator)
     )]
    public async void Command_ShouldThrowError_WhenMethodHandleIsCalledWithInvalidInputs(CreateCategoryInput input)
    {
        var loggerMock = _fixture.GetLoggerMock();
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        
        var command = new CreateCategory(unitOfWorkMock.Object, loggerMock.Object, repositoryMock.Object);
    
        Func<Task> task = async () => await command.Handle(input, CancellationToken.None);
    
        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage("Category is invalid");
        
        loggerMock.VerifyLog(LogLevel.Error, Times.Exactly(1));
        unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never);
        repositoryMock.Verify(repository => repository.Create(
            It.IsAny<Catalog.Domain.Entities.Category>()),
            Times.Never
        );
    }
}