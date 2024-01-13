using Flixer.Catalog.Application.Dtos.InputModel.Category;
using Flixer.Catalog.UnitTest.Application.UseCases.Category.Common;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.CreateCategoryUseCase;

[CollectionDefinition(nameof(CreateCategoryUseCaseTestFixture))]
public class CreateCategoryUseCaseTestFixtureCollection : ICollectionFixture<CreateCategoryUseCaseTestFixture>
{

}

public class CreateCategoryUseCaseTestFixture : CategoryUseCasesBaseFixture
{

    public CreateCategoryInputModel GetInput()
    {
        return new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    }

    public CreateCategoryInputModel GetInvalidInputShortName()
    {
        var invalidInputShortName = GetInput();
        invalidInputShortName.Name = invalidInputShortName.Name[..2];

        return invalidInputShortName;
    }

    public CreateCategoryInputModel GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = GetInput();
        var tooLongNameForCategory = Faker.Commerce.ProductName();
        while (tooLongNameForCategory.Length <= 255)
            tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";
        invalidInputTooLongName.Name = tooLongNameForCategory;

        return invalidInputTooLongName;
    }

    public CreateCategoryInputModel GetInvalidCategoryInputNull()
    {
        var invalidInputDescriptionNull = GetInput();
        invalidInputDescriptionNull.Description = null!;

        return invalidInputDescriptionNull;
    }

    public CreateCategoryInputModel GetInvalidInputTooLongDescription()
    {
        var invalidInputTooLongDescription = GetInput();
        var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();
        while (tooLongDescriptionForCategory.Length <= 10000)
            tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";
        invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;

        return invalidInputTooLongDescription;
    }
}