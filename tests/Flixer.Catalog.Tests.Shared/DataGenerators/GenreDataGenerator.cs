using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.Common.Input.Genre;
using Flixer.Catalog.Domain.Enums;

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
    
    public ListGenresInput GetListGenresInput()
    {
        var random = new Random();
        
        return new ListGenresInput(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ?
                SearchOrder.Asc : SearchOrder.Desc
        );
    }
}