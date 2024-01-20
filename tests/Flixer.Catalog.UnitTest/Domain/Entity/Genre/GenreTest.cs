using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Common.Tests.Fixture.Genre;
using DomainEntity = Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.UnitTest.Domain.Entity.Genre;

[Collection(nameof(GenreTestFixture))]
public class GenreTest
{
    private readonly GenreTestFixture _fixture;

    public GenreTest(GenreTestFixture fixture)
        => _fixture = fixture;

    [Fact]
    [Trait("Domain", "Genre - Aggregates")]
    public void Genre_ShouldHaveExpectedProperties_WhenInstantiated()
    {
        var genreName = _fixture.GetValidName();

        var datetimeBefore = DateTime.Now;

        var genre = new DomainEntity.Genre(genreName);

        var datetimeAfter = DateTime.Now.AddSeconds(1);

        genre.Should().NotBeNull();
        genre.Id.Should().NotBeEmpty();
        genre.IsActive.Should().BeTrue();
        genre.Name.Should().Be(genreName);
        genre.CreatedAt.Should().NotBeSameDateAs(default);
        (genre.CreatedAt <= datetimeAfter).Should().BeTrue();
        (genre.CreatedAt >= datetimeBefore).Should().BeTrue();
    }

    [Theory]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Genre_ShouldThrowError_WhenInstantiatedWithNameEmpty(string? name)
    {
        var action = () => new DomainEntity.Genre(name!);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Theory]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Genre_ShouldThrowError_WhenInstantiatedWithIsActive(bool isActive)
    {
        var genreName = _fixture.GetValidName();

        var datetimeBefore = DateTime.Now;

        var genre = new DomainEntity.Genre(genreName, isActive);

        var datetimeAfter = DateTime.Now.AddSeconds(1);

        genre.Should().NotBeNull();
        genre.Id.Should().NotBeEmpty();
        genre.Name.Should().Be(genreName);
        genre.IsActive.Should().Be(isActive);
        genre.CreatedAt.Should().NotBeSameDateAs(default);
        (genre.CreatedAt <= datetimeAfter).Should().BeTrue();
        (genre.CreatedAt >= datetimeBefore).Should().BeTrue();
    }

    [Theory]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Genre_ShouldIsActiveBeTrue_WhenMethodActivateIsCalled(bool isActive)
    {
        var genre = _fixture.GetExampleGenre(isActive);
        var oldName = genre.Name;

        genre.Activate();

        genre.Should().NotBeNull();
        genre.Id.Should().NotBeEmpty();
        genre.Name.Should().Be(oldName);
        genre.IsActive.Should().BeTrue();
        genre.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Genre_ShouldIsActiveBeFalse_WhenMethodDeactivateIsCalled(bool isActive)
    {
        var genre = _fixture.GetExampleGenre(isActive);
        var oldName = genre.Name;

        genre.Deactivate();

        genre.Should().NotBeNull();
        genre.Id.Should().NotBeEmpty();
        genre.Name.Should().Be(oldName);
        genre.IsActive.Should().BeFalse();
        genre.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact]
    [Trait("Domain", "Genre - Aggregates")]
    public void Genre_ShouldUpdate_WhenMethodUpdateIsCalledWithParameters()
    {
        var genre = _fixture.GetExampleGenre();
        var newName = _fixture.GetValidName();

        var oldIsActive = genre.IsActive;

        genre.Update(newName);

        genre.Should().NotBeNull();
        genre.Id.Should().NotBeEmpty();
        genre.Name.Should().Be(newName);
        genre.IsActive.Should().Be(oldIsActive);
        genre.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Genre_ShouldThrowError_WhenNameIsEmpty(string? name)
    {
        var genre = _fixture.GetExampleGenre();

        var action = () => genre.Update(name!);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Fact]
    [Trait("Domain", "Genre - Aggregates")]
    public void Genre_ShouldAddCategory_WhenMethoAddCategoryIsCalled()
    {
        var genre = _fixture.GetExampleGenre();
        var categoryGuid = Guid.NewGuid();

        genre.AddCategory(categoryGuid);

        genre.Categories.Should().HaveCount(1);
        genre.Categories.Should().Contain(categoryGuid);
    }

    [Fact]
    [Trait("Domain", "Genre - Aggregates")]
    public void Genre_ShouldAddTwoCategories_WhenMethodAddCategoryIsCalledTwoTimes()
    {
        var genre = _fixture.GetExampleGenre();

        var categoryGuid1 = Guid.NewGuid();
        var categoryGuid2 = Guid.NewGuid();

        genre.AddCategory(categoryGuid1);
        genre.AddCategory(categoryGuid2);

        genre.Categories.Should().HaveCount(2);
        genre.Categories.Should().Contain(categoryGuid1);
        genre.Categories.Should().Contain(categoryGuid2);
    }

    [Fact]
    [Trait("Domain", "Genre - Aggregates")]
    public void Genre_ShouldRemoveCategory_WhenMethodRemoveCategoryIsCalled()
    {
        var exampleGuid = Guid.NewGuid();
        var genre = _fixture.GetExampleGenre(
            categoriesIdsList: new List<Guid>()
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                exampleGuid,
                Guid.NewGuid(),
                Guid.NewGuid()
            }
        );

        genre.RemoveCategory(exampleGuid);

        genre.Categories.Should().HaveCount(4);
        genre.Categories.Should().NotContain(exampleGuid);
    }

    [Fact]
    [Trait("Domain", "Genre - Aggregates")]
    public void Genre_ShouldRemoveAllCategories_WhenMethodRemoveAllCategoriesIsCalled()
    {
        var genre = _fixture.GetExampleGenre(
            categoriesIdsList: new List<Guid>()
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            }
        );

        genre.RemoveAllCategories();

        genre.Categories.Should().HaveCount(0);
    }
}
