using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.UnitTest.Fixture.Domain;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Category;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryFixture))]
public class CreateCategoryCommandFixtureCollection : ICollectionFixture<CreateCategoryFixture>
{
    
}

public class CreateCategoryFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; } = new();
    
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Commands.Category.CreateCategory>> GetLoggerMock() => new();
    
    public CreateCategoryInput GetInputCreate()
     {
         return new CreateCategoryInput(
             CategoryFixture.GetValidCategoryName(), 
             CategoryFixture.GetValidCategoryDescription(), 
             CategoryFixture.GetRandomBoolean()
         );
     }
    
    public CreateCategoryInput GetInputCreateWithNameAndDescription()
    {
        return new CreateCategoryInput(
            CategoryFixture.GetValidCategoryName(), 
            CategoryFixture.GetValidCategoryDescription()
        );
    }
    
    public CreateCategoryInput GetInvaliCreatedInputTooLongName()
     {
         var invalidInputTooLongName = GetInputCreate();
         var tooLongNameForCategory = Faker.Commerce.ProductName();

         while (tooLongNameForCategory.Length <= 255)
             tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";

         invalidInputTooLongName.SetName(tooLongNameForCategory);

         return invalidInputTooLongName;
     }

     public CreateCategoryInput GetInvalidCreateInputTooLongDescription()
     {
         var invalidInputTooLongDescription = GetInputCreate();
         var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();

         while (tooLongDescriptionForCategory.Length <= 10000)
             tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";

         invalidInputTooLongDescription.SetDescription(tooLongDescriptionForCategory);

         return invalidInputTooLongDescription;
     }
     
     public CreateCategoryInput GetInvalidCreateInputShortName()
     {
         var invalidInputShortName = GetInputCreate();
         invalidInputShortName.SetName(invalidInputShortName.Name[..2]);

         return invalidInputShortName;
     }
}