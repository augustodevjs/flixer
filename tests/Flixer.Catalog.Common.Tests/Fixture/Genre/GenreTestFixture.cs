using Moq;
using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Common.Tests.fixture;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.Contracts.UnityOfWork;
using Flixer.Catalog.Application.Dtos.InputModel.Genre;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Common.Tests.Fixture.Genre;
public class GenreTestFixture : BaseFixture
{
    public Mock<IUnityOfWork> GetUnitOfWorkMock()
        => new();

    public Mock<IGenreRepository> GetGenreRepositoryMock()
        => new();

    public Mock<ICategoryRepository> GetCategoryRepositoryMock()
        => new();

    public string GetValidGenreName()
       => Faker.Commerce.Categories(1)[0];

    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;

    public DomainEntity.Genre GetExampleGenre(bool? isActive = null, List<Guid>? categoriesIds = null)
    {
        var genre = new DomainEntity.Genre(GetValidGenreName(), isActive ?? GetRandomBoolean());
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
        return Enumerable.Range(1, count ?? new Random().Next(1, 10))
            .Select(_ => Guid.NewGuid()).ToList();
    }

    public CreateGenreInputModel GetGenreCreateInput()
        => new(
            GetValidGenreName(),
            GetRandomBoolean()
        );

    public CreateGenreInputModel GetGenreCreateInput(string? name)
        => new(
            name!,
            GetRandomBoolean()
        );

    public CreateGenreInputModel GetGenreCreateInputWithCategories()
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

    public ListGenresInputModel GetExampleInput()
    {
        var random = new Random();

        return new ListGenresInputModel(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
        );
    }
}