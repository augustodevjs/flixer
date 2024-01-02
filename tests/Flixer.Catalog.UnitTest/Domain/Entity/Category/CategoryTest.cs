using Flixer.Catalog.Domain.Exceptions;
using DomainEntity = Flixer.Catalog.Domain.Entities;
namespace Flixer.Catalog.UnitTest.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;

    public CategoryTest(CategoryTestFixture categoryTestFixture)
    {
        _categoryTestFixture = categoryTestFixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;
        
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
        (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (category.IsActive).Should().BeTrue();
    }
    
    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var dateTimeBefore = DateTime.Now;
        
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
        (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (category.IsActive).Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateThrowErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void InstantiateThrowErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = () => new DomainEntity.Category(name!, validCategory.Description);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }
    
    [Fact(DisplayName = nameof(InstantiateThrowErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateThrowErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = () => new DomainEntity.Category(validCategory.Name, null!);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should not be null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThanThreeCharecters))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetWhenNameIsLessThanThreeCharecters), parameters: 10)]
    public void InstantiateErrorWhenNameIsLessThanThreeCharecters(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should have at least 3 characters");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThanTwoHundredFiftyFiveCharacters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThanTwoHundredFiftyFiveCharacters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);

        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should have less or equal 255 characters");
    }
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThanOneThousandCharacters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThanOneThousandCharacters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();

        while (invalidDescription.Length <= 10000)
            invalidDescription = $"{invalidDescription}{_categoryTestFixture.Faker.Commerce.ProductDescription()}";

        Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should have less or equal 10.000 characters");
    }
    
    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
        
        category.Activate();

        category.IsActive.Should().BeTrue();
    }
    
    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
        
        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }
        
    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var categoryNewValues = _categoryTestFixture.GetValidCategory();

        category.Update(categoryNewValues.Name, categoryNewValues.Description);

        category.Name.Should().Be(categoryNewValues.Name);
        category.Description.Should().Be(categoryNewValues.Description);
    }
    
    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = _categoryTestFixture.GetValidCategory();

        var newName = _categoryTestFixture.GetValidCategoryName();
        var currentDescription = category.Description;
        
        category.Update(newName);

        category.Name.Should().Be(newName);
        category.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = _categoryTestFixture.GetValidCategory();
        
        Action action = () => category.Update(name!) ;

        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThanThreeCharecters))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetWhenNameIsLessThanThreeCharecters), parameters: 10)]
    public void UpdateErrorWhenNameIsLessThanThreeCharecters(string invalidName)
    {
        var category = _categoryTestFixture.GetValidCategory();

        Action action = () => category.Update(invalidName!);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should have at least 3 characters");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThanTwoHundredFiftyFiveCharacters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThanTwoHundredFiftyFiveCharacters()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);

        Action action = () => category.Update(invalidName!);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should have less or equal 255 characters");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThanOneThousandCharacters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThanOneThousandCharacters()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();

        while (invalidDescription.Length <= 10000)
            invalidDescription = $"{invalidDescription}{_categoryTestFixture.Faker.Commerce.ProductDescription()}";

        Action action = () => category.Update("New Category Name", invalidDescription!);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should have less or equal 10.000 characters");
    }

    public static IEnumerable<object[]> GetWhenNameIsLessThanThreeCharecters(int numberOfTests)
    {
        var fixture = new CategoryTestFixture();

        for (int i = 0; i < numberOfTests; i++)
        {
            var isOdd = i % 2 == 1;
            yield return new object[] { fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)] };
        }
    }
}