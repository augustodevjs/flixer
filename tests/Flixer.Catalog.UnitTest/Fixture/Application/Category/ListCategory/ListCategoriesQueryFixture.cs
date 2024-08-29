using Moq;
using Xunit;
using Flixer.Catalog.Domain.Enums;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.UnitTest.Fixture.Domain.Category;
using Flixer.Catalog.Application.Queries.Category.ListCategories;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.ListCategory;

[CollectionDefinition(nameof(ListCategoriesQueryFixture))]
public class ListCategoryQueryFixtureCollection : ICollectionFixture<ListCategoriesQueryFixture>
{
    
}

public class ListCategoriesQueryFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<ListCategoriesQueryHandler>> GetLogger() => new();

     public List<Catalog.Domain.Entities.Category> GetExampleCategoriesList(int length = 10)
     {
         var list = new List<Catalog.Domain.Entities.Category>();

         for (var i = 0; i < length; i++)
             list.Add(CategoryFixture.GetValidCategory());

         return list;
     }
     
     public ListCategoriesQuery GetListInput()
     {
         var random = new Random();

         return new ListCategoriesQuery(
             page: random.Next(1, 10),
             perPage: random.Next(15, 100),
             search: Faker.Commerce.ProductName(),
             sort: Faker.Commerce.ProductName(),
             dir: random.Next(0, 10) > 5 ?
                 SearchOrder.Asc : SearchOrder.Desc
         );
     }
}