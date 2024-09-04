using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Tests.Shared.Helpers;
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
    
    // [Fact]
    // [Trait("Application", "GetGenre - Query")]
    // public async Task Query_ShouldGetGenre()
    // {
    //     var loggerMock = _fixture.GetLoggerMock();
    //     var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
    //     var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
    //     
    //     var exampleGenre = _fixture.DataGenerator.GetValidGenre();
    //     
    //     genreRepositoryMock.Setup(x => x.GetById(
    //         It.IsAny<Guid>()
    //     )).ReturnsAsync(exampleGenre);
    //     
    //     var input = new GetGenreInput(exampleGenre.Id);
    //
    //     var command = new GetGenre(
    //         loggerMock.Object,
    //         genreRepositoryMock.Object,
    //         categoryRepositoryMock.Object
    //     );
    //
    //     var output = await command.Handle(input, CancellationToken.None);
    //
    //     output.Should().NotBeNull();
    //     output.Id.Should().Be(exampleGenre.Id);
    //     output.Name.Should().Be(exampleGenre.Name);
    //     output.IsActive.Should().Be(exampleGenre.IsActive);
    //     output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
    //     output.Categories.Should().HaveCount(exampleGenre.Categories.Count);
    //     
    //     genreRepositoryMock.Verify(
    //         x => x.GetById(It.IsAny<Guid>()),
    //         Times.Once
    //     );
    // }

    // [Fact(DisplayName = nameof(ThrowWhenNotFound))]
    // [Trait("Application", "GetGenre - Use Cases")]
    // public async Task ThrowWhenNotFound()
    // {
    //     var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
    //     var exampleId = Guid.NewGuid();
    //     genreRepositoryMock.Setup(x => x.Get(
    //         It.Is<Guid>(x => x == exampleId),
    //         It.IsAny<CancellationToken>()
    //     )).ThrowsAsync(new NotFoundException(
    //         $"Genre '{exampleId}' not found"
    //     ));
    //     var useCase = new UseCase
    //         .GetGenre(genreRepositoryMock.Object);
    //     var input = new UseCase.GetGenreInput(exampleId);
    //
    //     var action = async ()
    //         => await useCase.Handle(input, CancellationToken.None);
    //     
    //     await action.Should().ThrowAsync<NotFoundException>()
    //         .WithMessage($"Genre '{exampleId}' not found");
    //     genreRepositoryMock.Verify(
    //         x => x.Get(
    //             It.Is<Guid>(x => x == exampleId),
    //             It.IsAny<CancellationToken>()
    //         ),
    //         Times.Once
    //     );
    // }
}