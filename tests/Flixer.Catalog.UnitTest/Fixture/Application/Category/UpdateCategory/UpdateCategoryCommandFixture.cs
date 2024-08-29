using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.UnitTest.Fixture.Domain.Category;
using Flixer.Catalog.Application.Commands.Category.UpdateCategory;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryCommandFixture))]
public class UpdateCategoryCommandFixtureCollection : ICollectionFixture<UpdateCategoryCommandFixture>
{
    
}

public class UpdateCategoryCommandFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; } = new();
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<UpdateCategoryCommandHandler>> GetLoggerMock() => new();
    
     public UpdateCategoryCommand GetInputUpdate(Guid? id = null)
     {
         return new UpdateCategoryCommand(
             id ?? Guid.NewGuid(),
             CategoryFixture.GetValidCategoryName(),
             CategoryFixture.GetValidCategoryDescription(),
             CategoryFixture.GetRandomBoolean()
         );
     }
     
     public UpdateCategoryCommand GetInvalidUpdateInputShortName()
     {
         var invalidInputShortName = GetInputUpdate();
         invalidInputShortName.Name = invalidInputShortName.Name[..2];

         return invalidInputShortName;
     }

     public UpdateCategoryCommand GetInvalidUpdateInputTooLongName()
     {
         var invalidInputTooLongName = GetInputUpdate();
         var tooLongNameForCategory = Faker.Commerce.ProductName();

         while (tooLongNameForCategory.Length <= 255)
             tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";

         invalidInputTooLongName.Name = tooLongNameForCategory;

         return invalidInputTooLongName;
     }

     public UpdateCategoryCommand GetInvalidUpdateInputTooLongDescription()
     {
         var invalidInputTooLongDescription = GetInputUpdate();
         var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();

         while (tooLongDescriptionForCategory.Length <= 10_000)
             tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";

         invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;

         return invalidInputTooLongDescription;
     }
}