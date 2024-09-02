using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Application.Common.Input.Category;

namespace Flixer.Catalog.Tests.Shared.DataGenerators;

public class CategoryDataGenerator : DataGeneratorBase
{
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
    
    public string GetNamesWithLessThan3Characters()
    {
        var name = Faker.Name.FirstName();

        if (name.Length >= 3)
            name = name.Substring(0, 2);

        return name;
    }

    public string GetDescriptionWithGreaterThan10_000Characters()
    {
        var description = Faker.Commerce.ProductDescription();

        while (description.Length <= 10_000)
            description = $"{description} {Faker.Commerce.ProductDescription()}";

        return description;
    }

    public Category GetValidCategory() => new(
        GetValidCategoryName(),
        GetValidCategoryDescription()
    );
    
    public CreateCategoryInput GetInputCreate()
    {
        return new CreateCategoryInput(
            GetValidCategoryName(), 
            GetValidCategoryDescription(), 
            GetRandomBoolean()
        );
    }
    
    public CreateCategoryInput GetInputCreateWithNameAndDescription()
    {
        return new CreateCategoryInput(
            GetValidCategoryName(), 
            GetValidCategoryDescription()
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
    
     public List<Category> GetExampleCategoriesList(int length = 10)
     {
         var list = new List<Category>();

         for (var i = 0; i < length; i++)
             list.Add(GetValidCategory());

         return list;
     }
         
     public ListCategoriesInput GetListInput()
     {
         var random = new Random();

         return new ListCategoriesInput(
             page: random.Next(1, 10),
             perPage: random.Next(15, 100),
             search: Faker.Commerce.ProductName(),
             sort: Faker.Commerce.ProductName(),
             dir: random.Next(0, 10) > 5 ?
                 SearchOrder.Asc : SearchOrder.Desc
         );
     }
         
     public UpdateCategoryInput GetInputUpdate(Guid? id = null)
     {
         return new UpdateCategoryInput(
             id ?? Guid.NewGuid(),
             GetValidCategoryName(),
             GetValidCategoryDescription(),
             GetRandomBoolean()
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