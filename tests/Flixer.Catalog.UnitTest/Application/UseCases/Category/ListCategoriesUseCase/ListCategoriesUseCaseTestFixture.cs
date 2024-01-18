using Flixer.Catalog.Application.Dtos.InputModel.Category;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;
using Flixer.Catalog.UnitTest.Application.UseCases.Category.Common;
using DomainEntity = Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.ListCategoriesUseCase;

[CollectionDefinition(nameof(ListCategoriesUseCaseTestFixture))]
public class ListCategoriesTestFixtureCollection
    : ICollectionFixture<ListCategoriesUseCaseTestFixture>
{ }

public class ListCategoriesUseCaseTestFixture : CategoryUseCasesBaseFixture
{
    public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10)
    {
        var list = new List<DomainEntity.Category>();

        for (int i = 0; i < length; i++)
            list.Add(GetExampleCategory());

        return list;
    }

    public ListCategoriesInputModel GetExampleInput()
    {
        var random = new Random();

        return new ListCategoriesInputModel(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ?
                SearchOrder.Asc : SearchOrder.Desc
        );
    }
}