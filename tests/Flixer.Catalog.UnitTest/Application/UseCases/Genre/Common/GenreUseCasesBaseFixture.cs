using Flixer.Catalog.UnitTest.Common;
using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.Repository;
using DomainEntity = Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Genre.Common;

public class GenreUseCasesBaseFixture : BaseFixture
{
    public Mock<IGenreRepository> GetGenreRepositoryMock()
        => new();

    public Mock<IUnityOfWork> GetUnitOfWorkMock()
        => new();

    public Mock<ICategoryRepository> GetCategoryRepositoryMock()
        => new();

    public string GetValidGenreName()
        => Faker.Commerce.Categories(1)[0];

    public DomainEntity.Genre GetExampleGenre(bool? isActive = null, List<Guid>? categoriesIds = null)
    {
        var genre = new DomainEntity.Genre(GetValidGenreName(),isActive ?? GetRandomBoolean());
        categoriesIds?.ForEach(genre.AddCategory);

        return genre;
    }

    public List<DomainEntity.Genre> GetExampleGenresList(int count = 10)
    {
        return Enumerable.Range(1, count).Select(_ =>
        {
            var genre = new DomainEntity.Genre(
                GetValidGenreName(),
                GetRandomBoolean()
            );

            GetRandomIdsList().ForEach(genre.AddCategory);

            return genre;
        }).ToList();
    }

    public List<Guid> GetRandomIdsList(int? count = null)
    {
        return Enumerable.Range(1, count ?? (new Random()).Next(1, 10))
            .Select(_ => Guid.NewGuid()).ToList();
    }
}
