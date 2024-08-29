using Xunit;

namespace Flixer.Catalog.UnitTest.Fixture.Domain.Category;

[CollectionDefinition(nameof(CategoryFixture))]
public class CategoryFixtureCollection : ICollectionFixture<CategoryFixture>
{
    
}

public class CategoryFixture : BaseFixture
{
    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
    
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

    public Catalog.Domain.Entities.Category GetValidCategory() => new(
        GetValidCategoryName(),
        GetValidCategoryDescription()
    );
}