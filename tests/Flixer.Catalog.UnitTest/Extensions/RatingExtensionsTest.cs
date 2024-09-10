using Xunit;
using FluentAssertions;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Domain.Extensions;

namespace Flixer.Catalog.UnitTest.Extensions;

public class RatingExtensionsTest
{
    [Theory]
    [Trait("Domain", "Rating - Extensions")]
    [InlineData("ER", Rating.ER)]
    [InlineData("L", Rating.L)]
    [InlineData("10", Rating.Rate10)]
    [InlineData("12", Rating.Rate12)]
    [InlineData("14", Rating.Rate14)]
    [InlineData("16", Rating.Rate16)]
    [InlineData("18", Rating.Rate18)]
    public void Extensions_ShouldReturnExpectedRating_WhenMethodToRatingIsCalled(string enumString, Rating expectedRating)
    {
        enumString.ToRating().Should().Be(expectedRating);
    }

    [Fact]
    [Trait("Domain", "Rating - Extensions")]
    public void Extensions_ShouldThrowArgumentOutOfRangeException_WhenInvalidString()
    {
        var action = () => "Invalid".ToRating();
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [Trait("Domain", "Rating - Extensions")]
    [InlineData(Rating.ER, "ER")]
    [InlineData(Rating.L, "L")]
    [InlineData(Rating.Rate10, "10")]
    [InlineData(Rating.Rate12, "12")]
    [InlineData(Rating.Rate14, "14")]
    [InlineData(Rating.Rate16, "16")]
    [InlineData(Rating.Rate18, "18")]
    public void Extensions_ShouldReturnExpectedRating_WhenMethodToRatingToStringIsCalled(Rating rating, string expectedString)
    {
        rating.ToStringSignal().Should().Be(expectedString);
    }
}