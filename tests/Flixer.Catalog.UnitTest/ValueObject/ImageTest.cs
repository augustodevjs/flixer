using Xunit;
using FluentAssertions;
using Flixer.Catalog.Domain.ValueObject;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.ValueObject;

public class ImageTest : DataGeneratorBase
{
    
    [Fact]
    [Trait("Domain", "Image - ValueObjects")]
    public void ValueObject_ShouldHaveExpectedProperties_WhenInstantiated()
    {
        var path = Faker.Image.PicsumUrl();

        var image = new Image(path);

        image.Path.Should().Be(path);
    }
    
    [Fact]
    [Trait("Domain", "Image - ValueObjects")]
    public void ValueObject_EqualsByPath()
    {
        var path = Faker.Image.PicsumUrl();
        
        var image = new Image(path);
        var sameImage = new Image(path);

        var isItEquals = image == sameImage;

        isItEquals.Should().BeTrue();
    }

    [Fact]
    [Trait("Domain", "Image - ValueObjects")]
    public void ValueObject_DifferentByPath()
    {
        var path = Faker.Image.PicsumUrl();
        var differentPath = Faker.Image.PicsumUrl();
        
        var image = new Image(path);
        var sameImage = new Image(differentPath);

        var isItDifferent = image != sameImage;

        isItDifferent.Should().BeTrue();
    }
}