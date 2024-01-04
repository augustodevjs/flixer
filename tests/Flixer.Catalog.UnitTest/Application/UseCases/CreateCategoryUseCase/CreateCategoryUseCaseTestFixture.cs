using Flixer.Catalog.UnitTest.Common;
using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Application.Contracts;
using Flixer.Catalog.Application.UseCases.Category.CreateCategory;

namespace Flixer.Catalog.UnitTest.Application.UseCases.CreateCategoryUseCase;

[CollectionDefinition(nameof(CreateCategoryUseCaseTestFixture))]
public class CreateCategoryUseCaseTestFixtureCollection : ICollectionFixture<CreateCategoryUseCaseTestFixture>
{

}

public class CreateCategoryUseCaseTestFixture : BaseFixture
{
    public Mock<IUnityOfWork> GetUnityOfWorkMock() => new();
    public Mock<ICategoryRepository> GetCategoryRepositoryMock() => new();

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

    public bool GetRandomBoolean()
    {
        return (new Random()).NextDouble() < 0.5;
    }

    public CreateCategoryInput GetInput()
    {
        return new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    }
}