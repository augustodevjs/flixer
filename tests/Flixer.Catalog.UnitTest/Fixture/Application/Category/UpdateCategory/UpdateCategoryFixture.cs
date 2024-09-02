using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.UnitTest.Fixture.Domain;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Category;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryFixture))]
public class UpdateCategoryCommandFixtureCollection : ICollectionFixture<UpdateCategoryFixture>
{
    
}

public class UpdateCategoryFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Commands.Category.UpdateCategory>> GetLoggerMock() => new();
    
     public UpdateCategoryInput GetInputUpdate(Guid? id = null)
     {
         return new UpdateCategoryInput(
             id ?? Guid.NewGuid(),
             CategoryFixture.GetValidCategoryName(),
             CategoryFixture.GetValidCategoryDescription(),
             CategoryFixture.GetRandomBoolean()
         );
     }
     
     public UpdateCategoryInput GetInvalidUpdateInputShortName()
     {
         var invalidInputShortName = GetInputUpdate();
         invalidInputShortName.Name = invalidInputShortName.Name[..2];

         return invalidInputShortName;
     }

     public UpdateCategoryInput GetInvalidUpdateInputTooLongName()
     {
         var invalidInputTooLongName = GetInputUpdate();
         var tooLongNameForCategory = Faker.Commerce.ProductName();

         while (tooLongNameForCategory.Length <= 255)
             tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";

         invalidInputTooLongName.Name = tooLongNameForCategory;

         return invalidInputTooLongName;
     }

     public UpdateCategoryInput GetInvalidUpdateInputTooLongDescription()
     {
         var invalidInputTooLongDescription = GetInputUpdate();
         var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();

         while (tooLongDescriptionForCategory.Length <= 10_000)
             tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";

         invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;

         return invalidInputTooLongDescription;
     }
}