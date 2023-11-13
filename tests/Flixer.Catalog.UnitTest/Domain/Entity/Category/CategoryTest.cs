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

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotEqual(default, category.CreatedAt);
        Assert.True(category.CreatedAt <  dateTimeAfter);
        Assert.True(category.CreatedAt >  dateTimeBefore);
        Assert.True(category.IsActive);
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

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotEqual(default, category.CreatedAt);
        Assert.True(category.CreatedAt <  dateTimeAfter);
        Assert.True(category.CreatedAt >  dateTimeBefore);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateThrowErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void InstantiateThrowErrorWhenNameIsEmpty(string? name)
    {
        Action action = () => new DomainEntity.Category(name!, "category description");

        var exception = Assert.Throws<EntityValidationException>(action);
        
        Assert.Equal("Name should not be empty or null", exception.Message);
    }
    
    [Fact(DisplayName = nameof(InstantiateThrowErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateThrowErrorWhenDescriptionIsNull()
    {
        Action action = () => new DomainEntity.Category("Category Name", null!);

        var exception = Assert.Throws<EntityValidationException>(action);
        
        Assert.Equal("Description should not be null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThanThreeCharecters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("ab")]
    public void InstantiateErrorWhenNameIsLessThanThreeCharecters(string invalidName)
    {
        Action action = () => new DomainEntity.Category(invalidName, "category description");

        var exception = Assert.Throws<EntityValidationException>(action);
        
        Assert.Equal("Name should have at least 3 characters", exception.Message);
    }
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThanTwoHundredFiftyFiveCharacters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThanTwoHundredFiftyFiveCharacters()
    {
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        
        Action action = () => new DomainEntity.Category(invalidName, "category description");

        var exception = Assert.Throws<EntityValidationException>(action);
        
        Assert.Equal("Name should have less or equal 255 characters", exception.Message);
    }
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThanOneThousandCharacters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThanOneThousandCharacters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
        
        Action action = () => new DomainEntity.Category("Category Name", invalidDescription);

        var exception = Assert.Throws<EntityValidationException>(action);
        
        Assert.Equal("Description should have less or equal 10.000 characters", exception.Message);
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
        
        Assert.True(category.IsActive);
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
        
        Assert.False(category.IsActive);
    }
    
    
}