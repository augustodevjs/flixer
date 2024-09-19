using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Tests.Shared.Helpers;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.Queries.Genre;
using Flixer.Catalog.Application.Common.Input.Genre;
using Flixer.Catalog.UnitTest.Fixture.Application.Genre.GetGenre;

namespace Flixer.Catalog.UnitTest.Application.Genre;

[Collection(nameof(GetGenreFixture))]
public class GetGenreTest
{
    private readonly GetGenreFixture _fixture;

    public GetGenreTest(GetGenreFixture fixture) =>
        _fixture = fixture;
    
    [Fact]
    [Trait("Application", "GetGenre - Query")]
    public async Task Query_ShouldGetGenreWithoutCategories()
    {
        var loggerMock = _fixture.GetLoggerMock();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        
        var exampleGenre = _fixture.GenreDataGenerator.GetValidGenre();
        
        genreRepositoryMock.Setup(x => x.GetById(
            It.IsAny<Guid>()
        )).ReturnsAsync(exampleGenre);
        
        var input = new GetGenreInput(exampleGenre.Id);
    
        var command = new GetGenre(
            loggerMock.Object,
            genreRepositoryMock.Object,
            categoryRepositoryMock.Object
        );
    
        var output = await command.Handle(input, CancellationToken.None);
    
        output.Should().NotBeNull();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(exampleGenre.Name);
        output.IsActive.Should().Be(exampleGenre.IsActive);
        output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
        output.Categories.Should().HaveCount(exampleGenre.Categories.Count);
        
        genreRepositoryMock.Verify(
            x => x.GetById(It.IsAny<Guid>()),
            Times.Once
        );
        
        categoryRepositoryMock.Verify(x => 
            x.GetListByIdsAsync(It.IsAny<List<Guid>>()), 
        Times.Never);
        
        loggerMock.VerifyLog(LogLevel.Information, Times.Never());
    }
    
    [Fact]
    [Trait("Application", "GetGenre - Query")]
    public async Task Query_ShouldGetGenreWithCategories()
    {
        var loggerMock = _fixture.GetLoggerMock();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var exampleCategoriesList = _fixture.CategoryDataGenerator.GetExampleCategoriesList();
        
        var exampleGenre = _fixture.GenreDataGenerator
            .GetValidGenre(true, exampleCategoriesList.Select(x => x.Id).ToList());
        
        genreRepositoryMock.Setup(x => x.GetById(
            It.IsAny<Guid>()
        )).ReturnsAsync(exampleGenre);
        
        categoryRepositoryMock.Setup(x =>
            x.GetListByIdsAsync(It.IsAny<List<Guid>>())
        ).ReturnsAsync(exampleCategoriesList);
        
        var input = new GetGenreInput(exampleGenre.Id);
        
        var command = new GetGenre(
            loggerMock.Object,
            genreRepositoryMock.Object,
            categoryRepositoryMock.Object
        );
        
        var output = await command.Handle(input, CancellationToken.None);
        
        output.Should().NotBeNull();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(exampleGenre.Name);
        output.IsActive.Should().Be(exampleGenre.IsActive);
        output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
        output.Categories.Should().HaveCount(exampleGenre.Categories.Count);
        
        foreach (var category in output.Categories)
        {
            var expectedCategory = exampleCategoriesList.Single(x => x.Id == category.Id);
            category.Name.Should().Be(expectedCategory.Name);
        }
        
        genreRepositoryMock.Verify(
            x => x.GetById(It.IsAny<Guid>()),
            Times.Once
        );
        
        categoryRepositoryMock.Verify(x => 
            x.GetListByIdsAsync(It.IsAny<List<Guid>>()), 
            Times.Once()
        );
        
        loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(1));
    }

    [Fact]
    [Trait("Application", "GetGenre - Use Cases")]
    public async Task Query_ShouldThrowError_WhenNotFoundGenre()
    {
        var loggerMock = _fixture.GetLoggerMock();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        
        var exampleId = Guid.NewGuid();
        
        var input = new GetGenreInput(exampleId);
    
        var command = new GetGenre(
            loggerMock.Object,
            genreRepositoryMock.Object,
            categoryRepositoryMock.Object
        );
    
        var action = async () =>  await command.Handle(input, CancellationToken.None);
        
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Genre '{exampleId}' not found.");
        
        genreRepositoryMock.Verify(
            x => x.GetById(It.IsAny<Guid>()),
            Times.Once
        );
        
        categoryRepositoryMock.Verify(
            x => x.GetListByIdsAsync(It.IsAny<List<Guid>>()), 
            Times.Never
        );
        
        loggerMock.VerifyLog(LogLevel.Warning, Times.Never());
    }
}