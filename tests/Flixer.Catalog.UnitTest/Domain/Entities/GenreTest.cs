using Xunit;
using FluentAssertions;
using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.UnitTest.Fixture.Domain;

namespace Flixer.Catalog.UnitTest.Domain.Entities;

[Collection(nameof(GenreFixture))]
public class GenreTest
{
    private readonly GenreFixture _fixture;

    public GenreTest(GenreFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    [Trait("Domain", "Genre - Aggregates")]
    public void Genre_ShouldHaveExpectedProperties_WhenInstantiated()
    {
        var genreName = _fixture.DataGenerator.GetValidName();

        var datetimeBefore = DateTime.Now;
        var genre = new Genre(genreName);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        genre.Should().NotBeNull();
        genre.Name.Should().Be(genreName);
        genre.IsActive.Should().BeTrue();
        genre.CreatedAt.Should().NotBeSameDateAs(default);
        (genre.CreatedAt >= datetimeBefore).Should().BeTrue();
        (genre.CreatedAt <= datetimeAfter).Should().BeTrue();
    }
    
    [Theory]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Genre_ShouldThrowError_WhenInstantiatedWithNameEmpty(string? name)
    {
        var action = () => new Genre(name!);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Genre is invalid");
    }
    
    [Theory]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Genre_ShouldHaveExpectedProperties_WhenInstantiateWithIsActive(bool isActive)
    {
        var genreName = _fixture.DataGenerator.GetValidName();

        var datetimeBefore = DateTime.Now;
        var genre = new Genre(genreName, isActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        genre.Should().NotBeNull();
        genre.Name.Should().Be(genreName);
        genre.IsActive.Should().Be(isActive);
        genre.CreatedAt.Should().NotBeSameDateAs(default);
        (genre.CreatedAt >= datetimeBefore).Should().BeTrue();
        (genre.CreatedAt <= datetimeAfter).Should().BeTrue();
    }
    
    [Theory]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Genre_ShouldActivate_WhenMethodActivateIsCalled(bool isActive)
    {
        var genre = _fixture.DataGenerator.GetValidGenre(isActive);
        
        genre.Activate();

        genre.Should().NotBeNull();
        genre.IsActive.Should().BeTrue();
        genre.Name.Should().Be(genre.Name);
        genre.CreatedAt.Should().NotBeSameDateAs(default);
    }
    
    [Theory]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Genre_ShouldDeactivate_WhenMethodDeactivateIsCalled(bool isActive)
    {
        var genre = _fixture.DataGenerator.GetValidGenre(isActive);

        genre.Deactivate();

        genre.Should().NotBeNull();
        genre.IsActive.Should().BeFalse();
        genre.Name.Should().Be(genre.Name);
        genre.CreatedAt.Should().NotBeSameDateAs(default);
    }
    
    [Fact]
    [Trait("Domain", "Genre - Aggregates")]
    public void Genre_ShouldUpdate_WhenUpdateMethodIsCalled()
    {
        var genre = _fixture.DataGenerator.GetValidGenre();
        var newName = _fixture.DataGenerator.GetValidName();

        genre.Update(newName);

        genre.Should().NotBeNull();
        genre.Name.Should().Be(newName);
        genre.IsActive.Should().Be(genre.IsActive);
        genre.CreatedAt.Should().NotBeSameDateAs(default);
    }
    
    [Theory]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Genre_ShouldThrowError_WhenUpdateWithNameEmpty(string? name)
    {
        var genre = _fixture.DataGenerator.GetValidGenre();

        var action = () => genre.Update(name!);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Genre is invalid");
    }
    
    [Fact]
    [Trait("Domain", "Genre - Aggregates")]
    public void Genre_ShouldAddCategory_WhenMethodIsCalled()
    {
        var genre = _fixture.DataGenerator.GetValidGenre();
        var categoryGuid = Guid.NewGuid();

        genre.AddCategory(categoryGuid);

        genre.Categories.Should().HaveCount(1);
        genre.Categories.Should().Contain(categoryGuid);
    }
    
    [Fact]
    [Trait("Domain", "Genre - Aggregates")]
    public void Genre_ShouldRemoveCategory_WhenMethodIsCalled()
    {
        var exampleGuid = Guid.NewGuid();
        var genre = _fixture.DataGenerator.GetValidGenre();
        
        genre.AddCategory(exampleGuid);

        genre.RemoveCategory(exampleGuid);

        genre.Categories.Should().HaveCount(0);
        genre.Categories.Should().NotContain(exampleGuid);
    }
    
    [Fact]
    [Trait("Domain", "Genre - Aggregates")]
    public void Genre_ShouldRemoveAllCategories_WhenMethodIsCalled()
    {
        var genre = _fixture.DataGenerator.GetValidGenre(
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