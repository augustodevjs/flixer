using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Genre.DeleteGenre;

[CollectionDefinition(nameof(DeleteGenreFixture))]
public class DeleteGenreFixtureCollection : ICollectionFixture<DeleteGenreFixture>
{
    
}

public class DeleteGenreFixture
{
    public GenreDataGenerator DataGenerator { get; } = new();
    public Mock<IGenreRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Commands.Genre.DeleteGenre>> GetLoggerMock() => new();
}