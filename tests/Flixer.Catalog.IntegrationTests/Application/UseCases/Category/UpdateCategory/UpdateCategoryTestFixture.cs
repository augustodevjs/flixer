﻿using Flixer.Catalog.Application.Dtos.InputModel.Category;
using Flixer.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace Flixer.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture>
{

}

public class UpdateCategoryTestFixture : CategoryUseCaseBaseFixture
{
    public UpdateCategoryInputModel GetValidInput(Guid? id = null)
         => new(
             id ?? Guid.NewGuid(),
             GetValidCategoryName(),
             GetRandomBoolean(),
             GetValidCategoryDescription()
         );

    public UpdateCategoryInputModel GetInvalidInputShortName()
    {
        var invalidInputShortName = GetValidInput();
        invalidInputShortName.Name =
            invalidInputShortName.Name[..2];
        return invalidInputShortName;
    }

    public UpdateCategoryInputModel GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = GetValidInput();
        var tooLongNameForCategory = Faker.Commerce.ProductName();
        while (tooLongNameForCategory.Length <= 255)
            tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";
        invalidInputTooLongName.Name = tooLongNameForCategory;
        return invalidInputTooLongName;
    }

    public UpdateCategoryInputModel GetInvalidInputTooLongDescription()
    {
        var invalidInputTooLongDescription = GetValidInput();
        var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();
        while (tooLongDescriptionForCategory.Length <= 10_000)
            tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";
        invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;
        return invalidInputTooLongDescription;
    }
}