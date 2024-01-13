using Flixer.Catalog.Application.Dtos.InputModel.Genre;
using Flixer.Catalog.UnitTest.Application.UseCases.Genre.Common;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Genre.CreateGenreUseCase;

[CollectionDefinition(nameof(CreateGenreTestFixture))]
public class CreateGenreTestFixtureCollection : ICollectionFixture<CreateGenreTestFixture>
{ 
}

public class CreateGenreTestFixture : GenreUseCasesBaseFixture
{
    public CreateGenreInputModel GetExampleInput()
        => new(
            GetValidGenreName(),
            GetRandomBoolean()
        );

    public CreateGenreInputModel GetExampleInput(string? name)
        => new(
            name!,
            GetRandomBoolean()
        );

    public CreateGenreInputModel GetExampleInputWithCategories()
    {
        var numberOfCategoriesIds = (new Random()).Next(1, 10);

        var categoriesIds = Enumerable.Range(1, numberOfCategoriesIds)
            .Select(_ => Guid.NewGuid())
            .ToList();

        return new(
            GetValidGenreName(),
            GetRandomBoolean(),
            categoriesIds
        );
    }
}
