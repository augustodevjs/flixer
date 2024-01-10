using Flixer.Catalog.UnitTest.Application.UseCases.Genre.Common;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Genre.UpdateGenreUseCase;

[CollectionDefinition(nameof(UpdateGenreTestFixture))]
public class UpdateGenreTestFixtureCollection : ICollectionFixture<UpdateGenreTestFixture>
{ 
}

public class UpdateGenreTestFixture : GenreUseCasesBaseFixture
{
}