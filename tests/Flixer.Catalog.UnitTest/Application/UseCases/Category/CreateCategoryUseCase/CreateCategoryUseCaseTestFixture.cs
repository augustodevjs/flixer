using Flixer.Catalog.UnitTest.Application.Common;
using Flixer.Catalog.Application.UseCases.Category.CreateCategory;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.CreateCategoryUseCase;

[CollectionDefinition(nameof(CreateCategoryUseCaseTestFixture))]
public class CreateCategoryUseCaseTestFixtureCollection : ICollectionFixture<CreateCategoryUseCaseTestFixture>
{

}

public class CreateCategoryUseCaseTestFixture : CategoryUseCasesBaseFixture
{

    public CreateCategoryInput GetInput()
    {
        return new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    }

    public CreateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetInput();
        invalidInputShortName.Name = invalidInputShortName.Name[..2];

        return invalidInputShortName;
    }

    public CreateCategoryInput GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = GetInput();
        var tooLongNameForCategory = Faker.Commerce.ProductName();
        while (tooLongNameForCategory.Length <= 255)
            tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";
        invalidInputTooLongName.Name = tooLongNameForCategory;

        return invalidInputTooLongName;
    }

    public CreateCategoryInput GetInvalidCategoryInputNull()
    {
        var invalidInputDescriptionNull = GetInput();
        invalidInputDescriptionNull.Description = null!;

        return invalidInputDescriptionNull;
    }

    public CreateCategoryInput GetInvalidInputTooLongDescription()
    {
        var invalidInputTooLongDescription = GetInput();
        var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();
        while (tooLongDescriptionForCategory.Length <= 10000)
            tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";
        invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;

        return invalidInputTooLongDescription;
    }
}