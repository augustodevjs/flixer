using Xunit; 
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.UnitTest.Fixture.Domain.Category;
using Flixer.Catalog.UnitTest.Fixture.Application.Category.ListCategory;
using Flixer.Catalog.UnitTest.Fixture.Application.Category.CreateCategory;
using Flixer.Catalog.UnitTest.Fixture.Application.Category.UpdateCategory;

namespace Flixer.Catalog.IntegrationTests.Fixtures.Repository;

[CollectionDefinition(nameof(CategoryRepositoryFixture))]
public class CategoryRepositoryFixtureCollection : ICollectionFixture<CategoryRepositoryFixture>
{
    
}

public class CategoryRepositoryFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; } = new();
    public ListCategoriesQueryFixture ListCategoriesQueryFixture { get; } = new();
    public CreateCategoryCommandFixture CreateCategoryCommandFixture { get; } = new();
    public UpdateCategoryCommandFixture UpdateCategoryCommandFixture { get; } = new();
    
    public List<Category> GetExampleCategoriesListWithNames(List<string> names)
        => names.Select(name =>
        {
            var category = CategoryFixture.GetValidCategory();
            category.Update(name);
            return category;
        }).ToList();
    
    public List<Category> CloneCategoriesListOrdered(
        List<Category> categoriesList,
        string orderBy,
        SearchOrder order
    )
    {
        var listClone = new List<Category>(categoriesList);
        
        var orderedEnumerable = (orderBy.ToLower(), order) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name)
                .ThenBy(x => x.Id),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
        };
        
        return orderedEnumerable.ToList();
    }
}