using Xunit;
using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.UnitTest.Fixture.Domain;

[CollectionDefinition(nameof(GenreFixture))]
public class GenreFixtureCollection : ICollectionFixture<GenreFixture>
{
    
}

public class GenreFixture : BaseFixture
{
    public string GetValidName()
    {
        return Faker.Commerce.Categories(1)[0];
    }

    public Genre GetValidGenre(bool isActive = true, List<Guid>? categoriesIdsList = null)
    {
        var genre = new Genre(GetValidName(), isActive);

        if (categoriesIdsList is null) return genre;
        
        foreach (var categoryId in categoriesIdsList)
        {
            genre.AddCategory(categoryId);
        }

        return genre;
    }
}