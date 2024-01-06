using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.UnitTest.Common;
using Flixer.Catalog.Domain.Repository;
using Contracts = Flixer.Catalog.Application.Contracts;

namespace Flixer.Catalog.UnitTest.Application.Common;

public class CategoryUseCasesBaseFixture : BaseFixture
{
    public Mock<ICategoryRepository> GetRepositoryMock()
        => new();

    public Mock<Contracts.IUnityOfWork> GetUnitOfWorkMock()
        => new();

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
        var categoryDescription =
            Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10_000)
            categoryDescription =
                categoryDescription[..10_000];
        return categoryDescription;
    }

    public Category GetExampleCategory()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );
}
