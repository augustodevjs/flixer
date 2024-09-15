using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Genre.UpdateGenre;

[CollectionDefinition(nameof(UpdateGenreFixture))]
public class UpdateGenreFixtureCollection : ICollectionFixture<UpdateGenreFixture>
{
    
}

public class UpdateGenreFixture
{
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
    public GenreDataGenerator DataGenerator { get; } = new();
    public Mock<IGenreRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Commands.Genre.UpdateGenre>> GetLoggerMock() => new();
}