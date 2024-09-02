using Flixer.Catalog.Application.Commands.Genre;
using Flixer.Catalog.Tests.Shared.Helpers;
using Xunit;
using Flixer.Catalog.UnitTest.Fixture.Application.Genre.CreateGenre;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

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
}