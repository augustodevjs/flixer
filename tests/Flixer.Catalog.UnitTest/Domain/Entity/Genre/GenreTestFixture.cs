using Flixer.Catalog.UnitTest.Common;
using DomainEntity = Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.UnitTest.Domain.Entity.Genre;

[CollectionDefinition(nameof(GenreTestFixture))]
public class GenreTestFixtureCollection : ICollectionFixture<GenreTestFixture>
{ 
}

public class GenreTestFixture : BaseFixture
{
    public string GetValidName()
       => Faker.Commerce.Categories(1)[0];

    public DomainEntity.Genre GetExampleGenre(bool isActive = true, List<Guid>? categoriesIdsList = null)
    {
        var genre = new DomainEntity.Genre(GetValidName(), isActive);

        if (categoriesIdsList is not null)
        {
            foreach (var categoryId in categoriesIdsList)
            {
                genre.AddCategory(categoryId);
            }
        }

        return genre;
    }
}
