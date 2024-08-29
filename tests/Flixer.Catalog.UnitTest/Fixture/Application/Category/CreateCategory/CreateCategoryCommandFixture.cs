using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.UnitTest.Fixture.Domain.Category;
using Flixer.Catalog.Application.Commands.Category.CreateCategory;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryCommandFixture))]
public class CreateCategoryCommandFixtureCollection : ICollectionFixture<CreateCategoryCommandFixture>
{
    
}

public class CreateCategoryCommandFixture : BaseFixture
{
    public CategoryFixture CategoryFixture { get; } = new();
    
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<ILogger<CreateCategoryCommandHandler>> GetLoggerMock() => new();
    
    public CreateCategoryCommand GetInputCreate()
     {
         return new CreateCategoryCommand(
             CategoryFixture.GetValidCategoryName(), 
             CategoryFixture.GetValidCategoryDescription(), 
             CategoryFixture.GetRandomBoolean()
         );
     }
    
    public CreateCategoryCommand GetInputCreateWithNameAndDescription()
    {
        return new CreateCategoryCommand(
            CategoryFixture.GetValidCategoryName(), 
            CategoryFixture.GetValidCategoryDescription()
        );
    }
    
    public CreateCategoryCommand GetInvaliCreatedInputTooLongName()
     {
         var invalidInputTooLongName = GetInputCreate();
         var tooLongNameForCategory = Faker.Commerce.ProductName();

         while (tooLongNameForCategory.Length <= 255)
             tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";

         invalidInputTooLongName.SetName(tooLongNameForCategory);

         return invalidInputTooLongName;
     }

     public CreateCategoryCommand GetInvalidCreateInputTooLongDescription()
     {
         var invalidInputTooLongDescription = GetInputCreate();
         var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();

         while (tooLongDescriptionForCategory.Length <= 10000)
             tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";

         invalidInputTooLongDescription.SetDescription(tooLongDescriptionForCategory);

         return invalidInputTooLongDescription;
     }
     
     public CreateCategoryCommand GetInvalidCreateInputShortName()
     {
         var invalidInputShortName = GetInputCreate();
         invalidInputShortName.SetName(invalidInputShortName.Name[..2]);

         return invalidInputShortName;
     }
}