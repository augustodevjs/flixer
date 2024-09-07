using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.Common.Input.Genre;

namespace Flixer.Catalog.Tests.Shared.DataGenerators;

public class GenreDataGenerator : DataGeneratorBase
{
    public string? GetValidName()
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
    
    public List<Guid> GenerateGuids(int count)
    {
        var list = new List<Guid>();

        for (var i = 0; i < count; i++)
            list.Add(Guid.NewGuid());

        return list;
    }
    
    public CreateGenreInput GetInput() =>
        new(
            GetValidName(),
            GetRandomBoolean()
        );
    
    public CreateGenreInput GetInputInvalid(string? input) =>
        new(
            input,
            GetRandomBoolean()
        );
    
    public CreateGenreInput GetExampleInputWithGenre()
    {
        var numberOfCategoriesIds = (new Random()).Next(1, 10);
        var categoriesIds = Enumerable.Range(1, numberOfCategoriesIds)
            .Select(_ => Guid.NewGuid()).ToList();
        
        return new CreateGenreInput(
            GetValidName(),
            GetRandomBoolean(),
            categoriesIds
        );
    }
}