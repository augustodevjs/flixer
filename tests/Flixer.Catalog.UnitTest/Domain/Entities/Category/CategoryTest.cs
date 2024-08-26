using Flixer.Catalog.Domain.Exceptions;
using DomainEntity = Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.UnitTest.Domain.Fixtures.Category;

namespace Flixer.Catalog.UnitTest.Domain.Entities.Category;

[Collection(nameof(CategoryFixtureCollection))]
public class CategoryTest
{
    private readonly CategoryFixture _fixture;

    public CategoryTest(CategoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("Domain", "Category - Aggregates")]
    public void Category_ShouldHaveExpectedProperties_WhenInstantiated()
    {
        var validCategory = _fixture.GetValidCategory();

        var datetimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);

        var datetimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Id.Should().NotBeEmpty();
        category.IsActive.Should().BeTrue();
        category.Name.Should().Be(validCategory.Name);
        category.CreatedAt.Should().NotBeSameDateAs(default);
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        category.Description.Should().Be(validCategory.Description);
    }

    [Theory]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Category_ShouldHaveExpectedProperties_WhenInstantiatedWithIsActive(bool isActive)
    {
        var validCategory = _fixture.GetValidCategory();

        var datetimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);

        var datetimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Id.Should().NotBeEmpty();
        category.IsActive.Should().Be(isActive);
        category.Name.Should().Be(validCategory.Name);
        category.CreatedAt.Should().NotBeSameDateAs(default);
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        category.Description.Should().Be(validCategory.Description);
    }

    [Theory]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Category_ShouldThrowError_WhenInstantiatedWithNameEmpty(string? name)
    {
        var validCategory = _fixture.GetValidCategory();

        Action action = () => new DomainEntity.Category(name!, validCategory.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Category is invalid");
    }

    [Fact]
    [Trait("Domain", "Category - Aggregates")]
    public void Category_ShouldThrowError_WhenInstantiatedWithNullDescription()
    {
        var validCategory = _fixture.GetValidCategory();

        Action action = () => new DomainEntity.Category(validCategory.Name, null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Category is invalid");
    }

    [Fact]
    [Trait("Domain", "Category - Aggregates")]
    public void Category_ShouldThrowError_WhenInstantiatedWithNameIsLessThan3Characters()
    {
        var validCategory = _fixture.GetValidCategory();
        var invalidName = _fixture.GetNamesWithLessThan3Characters();
    
        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
    
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Category is invalid");
    }

    [Fact]
    [Trait("Domain", "Category - Aggregates")]
    public void Category_ShouldThrowError_WhenInstantiatedWithNameIsGreaterThan255Characters()
    {
        var validCategory = _fixture.GetValidCategory();
        var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Category is invalid");
    }

    [Fact]
    [Trait("Domain", "Category - Aggregates")]
    public void Category_ShouldThrowError_WhenInstantiatedWithDescriptionIsGreaterThan10_000Characters()
    {
        var invalidDescription = string.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        var validCategory = _fixture.GetValidCategory();

        Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Category is invalid");
    }

    [Fact]
    [Trait("Domain", "Category - Aggregates")]
    public void Category_ShouldIsActiveBeTrue_WhenMethodActivateIsCalled()
    {
        var validCategory = _fixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
        category.Activate();

        category.IsActive.Should().BeTrue();
    }

    [Fact]
    [Trait("Domain", "Category - Aggregates")]
    public void Category_ShouldIsActiveBeFalse_WhenMethodDeactivateIsCalled()
    {
        var validCategory = _fixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }

    [Fact]
    [Trait("Domain", "Category - Aggregates")]
    public void Category_ShouldUpdate_WhenMethodUpdateIsCalledWithParameters()
    {
        var category = _fixture.GetValidCategory();
        var categoryWithNewValues = _fixture.GetValidCategory();

        category.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);

        category.Name.Should().Be(categoryWithNewValues.Name);
        category.Description.Should().Be(categoryWithNewValues.Description);
    }

    [Fact]
    [Trait("Domain", "Category - Aggregates")]
    public void Category_ShouldUpdateOnlyName_WhenMethodUpdateIsCalled()
    {
        var category = _fixture.GetValidCategory();
        var newName = _fixture.GetValidCategoryName();
        var currentDescription = category.Description;

        category.Update(newName);

        category.Name.Should().Be(newName);
        category.Description.Should().Be(currentDescription);
    }

    [Theory]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Category_ShouldThrowError_WhenNameIsEmpty(string? name)
    {
        var category = _fixture.GetValidCategory();
        
        Action action = () => category.Update(name!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Category is invalid");
    }

    [Fact]
    [Trait("Domain", "Category - Aggregates")]
    public void Category_ShouldThrowError_WhenNameIsLessThan3Characters()
    {
        var category = _fixture.GetValidCategory();
        var invalidName = _fixture.GetNamesWithLessThan3Characters();
    
        Action action =
            () => category.Update(invalidName);
    
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Category is invalid");
    }

    [Fact]
    [Trait("Domain", "Category - Aggregates")]
    public void Category_ShouldThrowError_WhenNameIsGreaterThan255Characters()
    {
        var category = _fixture.GetValidCategory();
        var invalidName = _fixture.Faker.Lorem.Letter(256);

        Action action =
            () => category.Update(invalidName);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Category is invalid");
    }

    [Fact]
    [Trait("Domain", "Category - Aggregates")]
    public void Category_ShouldThrowError_WhenDescriptionIsGreaterThan10_000Characters()
    {
        var category = _fixture.GetValidCategory();
        var invalidDescription = _fixture.GetDescriptionWithGreaterThan10_000Characters();

        Action action = () => category.Update("Category New Name", invalidDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Category is invalid");
    }
}