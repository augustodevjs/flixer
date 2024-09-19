using Xunit;
using Flixer.Catalog.Application.Commands.Genre;
using Flixer.Catalog.UnitTest.Fixture.Application.Genre.UpdateGenre;

namespace Flixer.Catalog.UnitTest.Application.Genre;

[Collection(nameof(UpdateGenreFixture))]
public class UpdateGenreTest
{
    private readonly UpdateGenreFixture _fixture;

    public UpdateGenreTest(UpdateGenreFixture fixture)
    {
        _fixture = fixture;
    }
}