using Moq;
using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Common.Tests.fixture;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.Contracts.UnityOfWork;
using Flixer.Catalog.Application.Dtos.InputModel.Category;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Common.Tests.Fixture.Category;

public class CategoryTestFixture : BaseFixture
{
    public Mock<ICategoryRepository> GetRepositoryMock()
    => new();

    public Mock<IUnityOfWork> GetUnitOfWorkMock()
        => new();

    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;

    public string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10000)
            categoryDescription = categoryDescription[..10000];

        return categoryDescription;
    }

    public DomainEntity.Category GetValidCategory() => new(
        GetValidCategoryName(),
        GetValidCategoryDescription()
     );

    public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10)
    {
        var list = new List<DomainEntity.Category>();

        for (int i = 0; i < length; i++)
            list.Add(GetValidCategory());

        return list;
    }

    public List<DomainEntity.Category> GetExampleCategoriesListWithNames(
        List<string> names
    ) => names.Select(name =>
    {
        var category = GetValidCategory();
        category.Update(name);
        return category;
    }).ToList();

    public List<DomainEntity.Category> CloneCategoriesListOrdered(
           List<DomainEntity.Category> categoriesList,
           string orderBy,
           SearchOrder order
       )
    {
        var listClone = new List<DomainEntity.Category>(categoriesList);

        var orderedEnumerable = (orderBy.ToLower(), order) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
        };

        return orderedEnumerable.ToList();
    }

    public CreateCategoryInputModel GetInputCreate()
    {
        return new(
            GetValidCategoryName(), 
            GetValidCategoryDescription(), 
            GetRandomBoolean()
        );
    }

    public CreateCategoryInputModel GetInvalidCreateInputShortName()
    {
        var invalidInputShortName = GetInputCreate();
        invalidInputShortName.Name = invalidInputShortName.Name[..2];

        return invalidInputShortName;
    }

    public CreateCategoryInputModel GetInvaliCreatedInputTooLongName()
    {
        var invalidInputTooLongName = GetInputCreate();
        var tooLongNameForCategory = Faker.Commerce.ProductName();

        while (tooLongNameForCategory.Length <= 255)
            tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";

        invalidInputTooLongName.Name = tooLongNameForCategory;

        return invalidInputTooLongName;
    }

    public CreateCategoryInputModel GetInvalidCreateInputTooLongDescription()
    {
        var invalidInputTooLongDescription = GetInputCreate();
        var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();

        while (tooLongDescriptionForCategory.Length <= 10000)
            tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";

        invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;

        return invalidInputTooLongDescription;
    }

    public UpdateCategoryInputModel GetInputUpdate(Guid? id = null)
    {
        return new(
            id ?? Guid.NewGuid(),
            GetValidCategoryName(),
            GetRandomBoolean(),
            GetValidCategoryDescription()
        );
    }

    public UpdateCategoryInputModel GetInvalidUpdateInputShortName()
    {
        var invalidInputShortName = GetInputUpdate();
        invalidInputShortName.Name = invalidInputShortName.Name[..2];

        return invalidInputShortName;
    }

    public UpdateCategoryInputModel GetInvalidUpdateInputTooLongName()
    {
        var invalidInputTooLongName = GetInputUpdate();
        var tooLongNameForCategory = Faker.Commerce.ProductName();

        while (tooLongNameForCategory.Length <= 255)
            tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";

        invalidInputTooLongName.Name = tooLongNameForCategory;

        return invalidInputTooLongName;
    }

    public UpdateCategoryInputModel GetInvalidUpdateInputTooLongDescription()
    {
        var invalidInputTooLongDescription = GetInputUpdate();
        var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();

        while (tooLongDescriptionForCategory.Length <= 10_000)
            tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";

        invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;

        return invalidInputTooLongDescription;
    }

    public ListCategoriesInputModel GetListInput()
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