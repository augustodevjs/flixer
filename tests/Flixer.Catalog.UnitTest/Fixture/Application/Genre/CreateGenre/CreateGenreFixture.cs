using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Genre.CreateGenre;

[CollectionDefinition(nameof(CreateGenreFixture))]
public class CreateGenreFixtureCollection : ICollectionFixture<CreateGenreFixture>
{
    
}

public class CreateGenreFixture
{
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
    public GenreDataGenerator DataGenerator { get; } = new();
    public Mock<IGenreRepository> GetGenreRepositoryMock() => new();
    public Mock<ICategoryRepository> GetCategoryeRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Commands.Genre.CreateGenre>> GetLoggerMock() => new();
}