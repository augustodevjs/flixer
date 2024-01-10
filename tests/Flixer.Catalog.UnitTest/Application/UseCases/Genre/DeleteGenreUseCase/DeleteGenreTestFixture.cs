using Flixer.Catalog.UnitTest.Application.UseCases.Genre.Common;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Genre.DeleteGenreUseCase;

[CollectionDefinition(nameof(DeleteGenreTestFixture))]
public class DeleteGenreTestFixtureCollection : ICollectionFixture<DeleteGenreTestFixture>
{ 
}

public class DeleteGenreTestFixture : GenreUseCasesBaseFixture
{ 
}