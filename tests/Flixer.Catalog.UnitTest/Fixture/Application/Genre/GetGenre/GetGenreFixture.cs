﻿using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Genre.GetGenre;

[CollectionDefinition(nameof(GetGenreFixture))]
public class GetGenreFixtureCollection : ICollectionFixture<GetGenreFixture>
{
    
}


public class GetGenreFixture
{
    public GenreDataGenerator DataGenerator { get; } = new();
    public Mock<IGenreRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Queries.Genre.GetGenre>> GetLoggerMock() => new();
}