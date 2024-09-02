using Flixer.Catalog.Application.Common.Input.Genre;
using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Tests.Shared.DataGenerators;

public class GenreDataGenerator : DataGeneratorBase
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
    
    public CreateGenreInput GetInput() =>
        new(
            GetValidName(),
            GetRandomBoolean()
        );
}