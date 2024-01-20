using DomainEntity = Flixer.Catalog.Domain.Entities;

using Flixer.Catalog.Common.Tests.fixture;

namespace Flixer.Catalog.Common.Tests.Fixture.Genre;
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