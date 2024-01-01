using FluentAssertions;
using Flixer.Catalog.Domain.Exceptions;
using DomainEntity = Flixer.Catalog.Domain.Entities;
namespace Flixer.Catalog.UnitTest.Domain.Entity.Category;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validData = new 
        {
            Name = "category name",
            Description = "category description"
        };
        var dateTimeBefore = DateTime.Now;
        
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var dateTimeAfter = DateTime.Now;

        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt < dateTimeAfter).Should().BeTrue();
        (category.CreatedAt > dateTimeBefore).Should().BeTrue();
        (category.IsActive).Should().BeTrue();
    }
    
    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validData = new 
        {
            Name = "category name",
            Description = "category description"
        };
        var dateTimeBefore = DateTime.Now;
        
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        var dateTimeAfter = DateTime.Now;

        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt < dateTimeAfter).Should().BeTrue();
        (category.CreatedAt > dateTimeBefore).Should().BeTrue();
        (category.IsActive).Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateThrowErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void InstantiateThrowErrorWhenNameIsEmpty(string? name)
    {
        Action action = () => new DomainEntity.Category(name!, "category description");

        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }
    
    [Fact(DisplayName = nameof(InstantiateThrowErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateThrowErrorWhenDescriptionIsNull()
    {
        Action action = () => new DomainEntity.Category("Category Name", null!);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should not be null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThanThreeCharecters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("ab")]
    public void InstantiateErrorWhenNameIsLessThanThreeCharecters(string invalidName)
    {
        Action action = () => new DomainEntity.Category(invalidName, "category description");

        action.Should().Throw<EntityValidationException>().WithMessage("Name should have at least 3 characters");
    }
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThanTwoHundredFiftyFiveCharacters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThanTwoHundredFiftyFiveCharacters()
    {
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        
        Action action = () => new DomainEntity.Category(invalidName, "category description");

        action.Should().Throw<EntityValidationException>().WithMessage("Name should have less or equal 255 characters");
    }
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThanOneThousandCharacters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThanOneThousandCharacters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
        
        Action action = () => new DomainEntity.Category("Category Name", invalidDescription);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should have less or equal 10.000 characters");
    }
    
    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        var validData = new 
        {
            Name = "category name",
            Description = "category description"
        };
        
        var category = new DomainEntity.Category(validData.Name, validData.Description, false);
        
        category.Activate();

        category.IsActive.Should().BeTrue();
    }
    
    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validData = new 
        {
            Name = "category name",
            Description = "category description"
        };
        
        var category = new DomainEntity.Category(validData.Name, validData.Description, false);
        
        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }
        
    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var newValues = new { Name = "New Name", Description = "New Description" };
        
        category.Update(newValues.Name, newValues.Description);

        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(newValues.Description);
    }
    
    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var newValues = new { Name = "New Name" };
        var currentDescription = category.Description;
        
        category.Update(newValues.Name);

        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        Action action = () => category.Update(name!) ;

        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThanThreeCharecters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("ab")]
    public void UpdateErrorWhenNameIsLessThanThreeCharecters(string invalidName)
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        Action action = () => category.Update(invalidName!);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should have at least 3 characters");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThanTwoHundredFiftyFiveCharacters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThanTwoHundredFiftyFiveCharacters()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        Action action = () => category.Update(invalidName!);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should have less or equal 255 characters");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThanOneThousandCharacters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThanOneThousandCharacters()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());

        Action action = () => category.Update("New Category Name", invalidDescription!);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should have less or equal 10.000 characters");
    }
}