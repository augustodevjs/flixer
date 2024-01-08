using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.UnitTest.Application.Common;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;
using Flixer.Catalog.Application.UseCases.Category.ListCategories;

namespace Flixer.Catalog.UnitTest.Application.UseCases.ListCategoriesUseCase;

[CollectionDefinition(nameof(ListCategoriesUseCaseTestFixture))]
public class ListCategoriesTestFixtureCollection
    : ICollectionFixture<ListCategoriesUseCaseTestFixture>
{ }

public class ListCategoriesUseCaseTestFixture : CategoryUseCasesBaseFixture
{
    public List<Category> GetExampleCategoriesList(int length = 10)
    {
        var list = new List<Category>();

        for (int i = 0; i < length; i++)
            list.Add(GetExampleCategory());

        return list;
    }

    public ListCategoriesInput GetExampleInput()
    {
        var random = new Random();

        return new ListCategoriesInput(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ?
                SearchOrder.Asc : SearchOrder.Desc
        );
    }
}