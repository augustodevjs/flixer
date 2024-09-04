using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Tests.Shared.Helpers;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.Commands.Genre;
using Flixer.Catalog.UnitTest.Fixture.Application.Genre.CreateGenre;

namespace Flixer.Catalog.UnitTest.Application.Genre;

[Collection(nameof(CreateGenreFixture))]
public class CreateGenreTest
{
    private readonly CreateGenreFixture _fixture;

    public CreateGenreTest(CreateGenreFixture fixture) =>
        _fixture = fixture;

    [Fact]
    [Trait("Application", "CreateGenre - Command")]
    public async void Command_ShouldCreateGenre_WhenMethodHandleIsCalled()
    {
        var loggerMock = _fixture.GetLoggerMock();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryeRepositoryMock();
        
        var command = new CreateGenre(
            loggerMock.Object,
            genreRepositoryMock.Object,
            categoryRepositoryMock.Object
        );
        
        var input = _fixture.DataGenerator.GetInput();
        
        genreRepositoryMock.Setup(repo => repo.UnityOfWork.Commit())
            .ReturnsAsync(true);

        var datetimeBefore = DateTime.Now;
        var output = await command.Handle(input, CancellationToken.None);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.Categories.Should().HaveCount(0);
        (output.CreatedAt <= datetimeAfter).Should().BeTrue();
        (output.CreatedAt >= datetimeBefore).Should().BeTrue();
        output.CreatedAt.Should().NotBeSameDateAs(default);
        
        categoryRepositoryMock.Verify(x => 
            x.GetIdsListByIds(input.CategoriesIds!), Times.Never);
        
        genreRepositoryMock.Verify(x => 
            x.Create(It.IsAny<Catalog.Domain.Entities.Genre>()), Times.Once);
        
        genreRepositoryMock.Verify(repository => 
            repository.UnityOfWork.Commit(), Times.Once);
        
        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(1));
    }
    
    [Fact]
    [Trait("Application", "CreateGenre - Command")]
    public async Task Command_ShoulCreateWithRelatedCategories()
    {
        var loggerMock = _fixture.GetLoggerMock();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryeRepositoryMock();
        
        var input = _fixture.DataGenerator.GetExampleInputWithGenre();
        
        categoryRepositoryMock.Setup(
            x => x.GetIdsListByIds(It.IsAny<List<Guid>>()))
        .ReturnsAsync((IReadOnlyList<Guid>) input.CategoriesIds!);
        
        genreRepositoryMock.Setup(repo => repo.UnityOfWork.Commit())
            .ReturnsAsync(true);
        
        var command = new CreateGenre(
            loggerMock.Object,
            genreRepositoryMock.Object,
            categoryRepositoryMock.Object
        );

        var output = await command.Handle(input, CancellationToken.None);
        
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default);
        output.Categories.Should().HaveCount(input.CategoriesIds?.Count ?? 0);

        genreRepositoryMock.Verify(x => x.Create(
            It.IsAny<Catalog.Domain.Entities.Genre>()
        ), Times.Once);
        
        genreRepositoryMock.Verify(repository => 
            repository.UnityOfWork.Commit(), Times.Once);
        
        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(1));
    }

    [Fact]
    [Trait("Application", "CreateGenre - Command")]
    public async Task Command_ShouldThrowError_WhenCreateRelatedCategoryNotFound()
    {
        var loggerMock = _fixture.GetLoggerMock();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryeRepositoryMock();
        
        var input = _fixture.DataGenerator.GetExampleInputWithGenre();
        var exampleGuid = input.CategoriesIds![^1];
        
        categoryRepositoryMock.Setup(
            x => x.GetIdsListByIds(
                It.IsAny<List<Guid>>()
            )
        ).ReturnsAsync(
            input.CategoriesIds
                .FindAll(x => x != exampleGuid)
        );
        
        genreRepositoryMock.Setup(repo => repo.UnityOfWork.Commit())
            .ReturnsAsync(true);
        
        var command = new CreateGenre(
            loggerMock.Object,
            genreRepositoryMock.Object,
            categoryRepositoryMock.Object
        );

        var action = async () =>  await command.Handle(input, CancellationToken.None);
    
        await action.Should().ThrowAsync<RelatedAggregateException>()
            .WithMessage($"Related category id (or ids) not found: {exampleGuid}");
        
        categoryRepositoryMock.Verify(x =>
            x.GetIdsListByIds(
                It.IsAny<List<Guid>>()
            ), Times.Once
        );
        
        genreRepositoryMock.Verify(repository => 
            repository.UnityOfWork.Commit(), Times.Never);
        
        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(0));
    }
    
    [Theory]
    [Trait("Application", "CreateGenre - Command")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public async Task Command_ShouldThrowError_WhenNameIsInvalid(string name)
    {
        var loggerMock = _fixture.GetLoggerMock();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryeRepositoryMock();
        
        var input = _fixture.DataGenerator.GetInputInvalid(name);
        
        var command = new CreateGenre(
            loggerMock.Object,
            genreRepositoryMock.Object,
            categoryRepositoryMock.Object
        );
    
        var action = async () =>  await command.Handle(input, CancellationToken.None);
    
        await action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage($"Genre is invalid");
    }
}