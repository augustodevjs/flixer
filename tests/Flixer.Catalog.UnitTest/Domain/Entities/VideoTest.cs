using Xunit;
using FluentAssertions;
using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.UnitTest.Fixture.Domain;

namespace Flixer.Catalog.UnitTest.Domain.Entities;

[Collection(nameof(VideoFixture))]
public class VideoTest
{
    private readonly VideoFixture _fixture;

    public VideoTest(VideoFixture fixture)
        => _fixture = fixture;
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldHaveExpectedProperties_WhenInstantiated()
    {
        var expectedRating = _fixture.DataGenerator.GetRandomRating();
        var expectedTitle = _fixture.DataGenerator.GetValidTitle();
        var expectedOpened = _fixture.DataGenerator.GetRandomBoolean();
        var expectedDuration = _fixture.DataGenerator.GetValidDuration();
        var expectedPublished = _fixture.DataGenerator.GetRandomBoolean();
        var expectedDescription = _fixture.DataGenerator.GetValidDescription();
        var expectedYearLaunched = _fixture.DataGenerator.GetValidYearLaunched();

        var expectedCreatedDate = DateTime.Now;
        
        var video = new Video(
            expectedTitle,
            expectedDescription,
            expectedYearLaunched,
            expectedOpened,
            expectedPublished,
            expectedDuration,
            expectedRating
        );

        video.Title.Should().Be(expectedTitle);
        video.Rating.Should().Be(expectedRating);
        video.Opened.Should().Be(expectedOpened);
        video.Duration.Should().Be(expectedDuration);
        video.Published.Should().Be(expectedPublished);
        video.Description.Should().Be(expectedDescription);
        video.YearLaunched.Should().Be(expectedYearLaunched);
        video.CreatedAt.Should().BeCloseTo(expectedCreatedDate, TimeSpan.FromSeconds(10));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldThrowException_WhenTitleIsNotValid(string title)
    {
        var expectedRating = _fixture.DataGenerator.GetRandomRating();
        var expectedOpened = _fixture.DataGenerator.GetRandomBoolean();
        var expectedDuration = _fixture.DataGenerator.GetValidDuration();
        var expectedPublished = _fixture.DataGenerator.GetRandomBoolean();
        var expectedDescription = _fixture.DataGenerator.GetValidDescription();
        var expectedYearLaunched = _fixture.DataGenerator.GetValidYearLaunched();

        var action = () => new Video(
            title,
            expectedDescription,
            expectedYearLaunched,
            expectedOpened,
            expectedPublished,
            expectedDuration,
            expectedRating
        );
    
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Video is invalid");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldThrowException_WhenDescriptionIsNotValid(string description)
    {
        var expectedRating = _fixture.DataGenerator.GetRandomRating();
        var expectedTitle = _fixture.DataGenerator.GetValidTitle();
        var expectedOpened = _fixture.DataGenerator.GetRandomBoolean();
        var expectedDuration = _fixture.DataGenerator.GetValidDuration();
        var expectedPublished = _fixture.DataGenerator.GetRandomBoolean();
        var expectedYearLaunched = _fixture.DataGenerator.GetValidYearLaunched();

        var action = () => new Video(
            expectedTitle,
            description,
            expectedYearLaunched,
            expectedOpened,
            expectedPublished,
            expectedDuration,
            expectedRating
        );
    
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Video is invalid");
    }
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldThrowException_WhenTitleIsTooLongAndDescriptionIsTooLong()
    {
        var expectedRating = _fixture.DataGenerator.GetRandomRating();
        var expectedTitle = _fixture.DataGenerator.GetTooLongTitle();
        var expectedOpened = _fixture.DataGenerator.GetRandomBoolean();
        var expectedDuration = _fixture.DataGenerator.GetValidDuration();
        var expectedPublished = _fixture.DataGenerator.GetRandomBoolean();
        var expectedDescription = _fixture.DataGenerator.GetTooLongDescription();
        var expectedYearLaunched = _fixture.DataGenerator.GetValidYearLaunched();

        var action = () => new Video(
            expectedTitle,
            expectedDescription,
            expectedYearLaunched,
            expectedOpened,
            expectedPublished,
            expectedDuration,
            expectedRating
        );

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Video is invalid");
    }
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldUpdate_WhenMethodIsCalled()
    {
        var video = _fixture.DataGenerator.GetValidVideo();
        var expectedTitle = _fixture.DataGenerator.GetValidTitle();
        var expectedOpened = _fixture.DataGenerator.GetRandomBoolean();
        var expectedDuration = _fixture.DataGenerator.GetValidDuration();
        var expectedPublished = _fixture.DataGenerator.GetRandomBoolean();
        var expectedDescription = _fixture.DataGenerator.GetValidDescription();
        var expectedYearLaunched = _fixture.DataGenerator.GetValidYearLaunched();

        video.Update(
            expectedTitle,
            expectedDescription,
            expectedYearLaunched,
            expectedOpened,
            expectedPublished,
            expectedDuration
        );

        video.Title.Should().Be(expectedTitle);
        video.Opened.Should().Be(expectedOpened);
        video.Duration.Should().Be(expectedDuration);
        video.Published.Should().Be(expectedPublished);
        video.Description.Should().Be(expectedDescription);
        video.YearLaunched.Should().Be(expectedYearLaunched);
    }
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldThrowException_WhenUpdateMethodIsCalledWithDescriptionTooLongAndTitleTooLong()
    {
        var video = _fixture.DataGenerator.GetValidVideo();
        var expectedTitle = _fixture.DataGenerator.GetTooLongTitle();
        var expectedOpened = _fixture.DataGenerator.GetRandomBoolean();
        var expectedDuration = _fixture.DataGenerator.GetValidDuration();
        var expectedPublished = _fixture.DataGenerator.GetRandomBoolean();
        var expectedDescription = _fixture.DataGenerator.GetTooLongDescription();
        var expectedYearLaunched = _fixture.DataGenerator.GetValidYearLaunched();

        var action = () => video.Update(
            expectedTitle,
            expectedDescription,
            expectedYearLaunched,
            expectedOpened,
            expectedPublished,
            expectedDuration
        );

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Video is invalid");
    }
}