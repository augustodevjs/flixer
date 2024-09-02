using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Genre.ListGenre;

[CollectionDefinition(nameof(ListGenreFixture))]
public class ListGenreFixtureCollection : ICollectionFixture<ListGenreFixture>
{
    
}

public class ListGenreFixture
{
    public GenreDataGenerator DataGenerator { get; } = new();
    public Mock<IGenreRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Queries.Genre.ListGenres>> GetLoggerMock() => new();
}