using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Tests.Shared.Helpers;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.Commands.Genre;
using Flixer.Catalog.Application.Common.Input.Genre;
using Flixer.Catalog.UnitTest.Fixture.Application.Genre.DeleteGenre;

namespace Flixer.Catalog.UnitTest.Application.Genre;

[Collection(nameof(DeleteGenreFixture))]
public class DeleteGenreTest
{
    private readonly DeleteGenreFixture _fixture;

    public DeleteGenreTest(DeleteGenreFixture fixture) =>
        _fixture = fixture;
    
    [Fact(DisplayName = nameof(DeleteGenre))]
    [Trait("Application", "DeleteGenre - Command")]
    public async Task Command_ShouldDeleteGenre()
    {
        var loggerMock = _fixture.GetLoggerMock();
        var genreRepositoryMock = _fixture.GetRepositoryMock();
        
        var exampleGenre = _fixture.DataGenerator.GetValidGenre();
        
        genreRepositoryMock.Setup(x => 
            x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(exampleGenre);
        
        genreRepositoryMock.Setup(repo => repo.UnityOfWork.Commit())
            .ReturnsAsync(true);
        
        var input = new DeleteGenreInput(exampleGenre.Id);
        
        var command = new DeleteGenre(
            genreRepositoryMock.Object,
            loggerMock.Object
        );

        await command.Handle(input, CancellationToken.None);

        genreRepositoryMock.Verify(x => 
            x.GetById(It.IsAny<Guid>()), Times.Once);
        
        genreRepositoryMock.Verify(x => 
            x.Delete(It.IsAny<Catalog.Domain.Entities.Genre>()), Times.Once);
        
        genreRepositoryMock.Verify(repository => 
            repository.UnityOfWork.Commit(), Times.Once);
        
        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(1));
    }
    
    [Fact]
    [Trait("Application", "DeleteGenre - Command")]
    public async Task Command_ShouldThrowError_WhenGenreNotFound()
    {
        var loggerMock = _fixture.GetLoggerMock();
        var genreRepositoryMock = _fixture.GetRepositoryMock();
        
        var exampleId = Guid.NewGuid();
        
        var input = new DeleteGenreInput(exampleId);
        
        var command = new DeleteGenre(
            genreRepositoryMock.Object,
            loggerMock.Object
        );
    
        var action = async () => await command.Handle(input, CancellationToken.None);
    
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Genre '{exampleId}' not found.");
        
        genreRepositoryMock.Verify(x => 
            x.GetById(It.IsAny<Guid>()), Times.Once);
        
        genreRepositoryMock.Verify(x => 
            x.Delete(It.IsAny<Catalog.Domain.Entities.Genre>()), Times.Never);
        
        genreRepositoryMock.Verify(repository => 
            repository.UnityOfWork.Commit(), Times.Never);
    }
}