using Xunit;
using FluentAssertions;
using Flixer.Catalog.Domain.Enums;
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
        
        video.Thumb.Should().BeNull();
        video.Banner.Should().BeNull();
        video.ThumbHalf.Should().BeNull();
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

        video.Thumb.Should().BeNull();
        video.Banner.Should().BeNull();
        video.ThumbHalf.Should().BeNull();
        video.Title.Should().Be(expectedTitle);
        video.Opened.Should().Be(expectedOpened);
        video.Duration.Should().Be(expectedDuration);
        video.Published.Should().Be(expectedPublished);
        video.Description.Should().Be(expectedDescription);
        video.YearLaunched.Should().Be(expectedYearLaunched);
    }
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldUpdateWithRating_WhenMethodIsCalled()
    {
        var video = _fixture.DataGenerator.GetValidVideo();
        var expectedRating = _fixture.DataGenerator.GetRandomRating();
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
            expectedDuration,
            expectedRating
        );

        video.Thumb.Should().BeNull();
        video.Banner.Should().BeNull();
        video.ThumbHalf.Should().BeNull();
        video.Title.Should().Be(expectedTitle);
        video.Opened.Should().Be(expectedOpened);
        video.Rating.Should().Be(expectedRating);
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
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldUpdateThumb_WhenMethodIsCalled()
    {
        var validVideo = _fixture.DataGenerator.GetValidVideo();
        var validImagePath = _fixture.DataGenerator.GetValidImagePath();

        validVideo.UpdateThumb(validImagePath);

        validVideo.Thumb.Should().NotBeNull();
        validVideo.Thumb!.Path.Should().Be(validImagePath);
    }
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldUpdateBanner_WhenMethodIsCalled()
    {
        var validVideo = _fixture.DataGenerator.GetValidVideo();
        var validImagePath = _fixture.DataGenerator.GetValidImagePath();

        validVideo.UpdateBanner(validImagePath);

        validVideo.Banner.Should().NotBeNull();
        validVideo.Banner!.Path.Should().Be(validImagePath);
    }
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldUpdateThumbHalf_WhenMethodIsCalled()
    {
        var validVideo = _fixture.DataGenerator.GetValidVideo();
        var validImagePath = _fixture.DataGenerator.GetValidImagePath();

        validVideo.UpdateThumbHalf(validImagePath);

        validVideo.ThumbHalf.Should().NotBeNull();
        validVideo.ThumbHalf!.Path.Should().Be(validImagePath);
    }
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldUpdateMedia_WhenMethodIsCalled()
    {
        var validVideo = _fixture.DataGenerator.GetValidVideo();
        var validPath = _fixture.DataGenerator.GetValidMediaPath();

        validVideo.UpdateMedia(validPath);

        validVideo.Media.Should().NotBeNull();
        validVideo.Events.Should().HaveCount(1);
        validVideo.Media!.FilePath.Should().Be(validPath);
    }
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldUpdateTrailer_WhenMethodIsCalled()
    {
        var validVideo = _fixture.DataGenerator.GetValidVideo();
        var validPath = _fixture.DataGenerator.GetValidMediaPath();

        validVideo.UpdateTrailer(validPath);

        validVideo.Trailer.Should().NotBeNull();
        validVideo.Trailer!.FilePath.Should().Be(validPath);
    }

    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldUpdateAsSentToEncode_WhenMethodIsCalled()
    {
        var validVideo = _fixture.DataGenerator.GetValidVideo();
        var validPath = _fixture.DataGenerator.GetValidMediaPath();
        
        validVideo.UpdateMedia(validPath);
        validVideo.UpdateAsSentToEncode();

        validVideo.Media!.Status.Should().Be(MediaStatus.Processing);
    }
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldThrowExceptionInUpdateAsSentToEncode_WhenThereIsNoMedia()
    {
        var validVideo = _fixture.DataGenerator.GetValidVideo();

        var action = () => validVideo.UpdateAsSentToEncode();

        action.Should().Throw<EntityValidationException>()
            .WithMessage("There is no Media");
    }
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldAddCategory_WhenMethodIsCalled()
    {
        var validVideo = _fixture.DataGenerator.GetValidVideo();
        var categoryIdExample = Guid.NewGuid();

        validVideo.AddCategory(categoryIdExample);

        validVideo.Categories.Should().HaveCount(1);
        validVideo.Categories[0].Should().Be(categoryIdExample);
    }

    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldRemoveCategory_WhenMethodIsCalled()
    {
        var categoryIdExample = Guid.NewGuid();
        var categoryIdExample2 = Guid.NewGuid();
        var validVideo = _fixture.DataGenerator.GetValidVideo();
        
        validVideo.AddCategory(categoryIdExample);
        validVideo.AddCategory(categoryIdExample2);

        validVideo.RemoveCategory(categoryIdExample);

        validVideo.Categories.Should().HaveCount(1);
        validVideo.Categories[0].Should().Be(categoryIdExample2);
    }
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldRemoveAllCategory_WhenMethodIsCalled()
    {
        var validVideo = _fixture.DataGenerator.GetValidVideo();
        var categoryIdExample = Guid.NewGuid();
        var categoryIdExample2 = Guid.NewGuid();
        
        validVideo.AddCategory(categoryIdExample);
        validVideo.AddCategory(categoryIdExample2);

        validVideo.RemoveAllCategories();

        validVideo.Categories.Should().HaveCount(0);
    }
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldAddGenre_WhenMethodIsCalled()
    {
        var exampleId = Guid.NewGuid();
        var validVideo = _fixture.DataGenerator.GetValidVideo();

        validVideo.AddGenre(exampleId);

        validVideo.Genres.Should().HaveCount(1);
        validVideo.Genres[0].Should().Be(exampleId);
    }

    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldRemoveGenre_WhenMethodIsCalled()
    {
        var exampleId = Guid.NewGuid();
        var exampleId2 = Guid.NewGuid();
        var validVideo = _fixture.DataGenerator.GetValidVideo();
        
        validVideo.AddGenre(exampleId);
        validVideo.AddGenre(exampleId2);

        validVideo.RemoveGenre(exampleId2);

        validVideo.Genres.Should().HaveCount(1);
        validVideo.Genres[0].Should().Be(exampleId);
    }

    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldRemoveAllGenre_WhenMethodIsCalled()
    {
        var exampleId = Guid.NewGuid();
        var exampleId2 = Guid.NewGuid();
        var validVideo = _fixture.DataGenerator.GetValidVideo();
        
        validVideo.AddGenre(exampleId);
        validVideo.AddGenre(exampleId2);

        validVideo.RemoveAllGenres();

        validVideo.Genres.Should().HaveCount(0);
    }
    
    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldAddCastMember_WhenMethodIsCalled()
    {
        var validVideo = _fixture.DataGenerator.GetValidVideo();
        var exampleId = Guid.NewGuid();

        validVideo.AddCastMember(exampleId);

        validVideo.CastMembers[0].Should().Be(exampleId);
        validVideo.CastMembers.Should().HaveCount(1);
    }

    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldRemoveCastMember_WhenMethodIsCalled()
    {
        var exampleId = Guid.NewGuid();
        var exampleId2 = Guid.NewGuid();
        var validVideo = _fixture.DataGenerator.GetValidVideo();
        
        validVideo.AddCastMember(exampleId);
        validVideo.AddCastMember(exampleId2);

        validVideo.RemoveCastMember(exampleId2);

        validVideo.CastMembers[0].Should().Be(exampleId);
        validVideo.CastMembers.Should().HaveCount(1);
    }

    [Fact]
    [Trait("Domain", "Video - Aggregate")]
    public void Video_ShouldRemoveAllCastMembers_WhenMethodIsCalled()
    {
        var exampleId = Guid.NewGuid();
        var exampleId2 = Guid.NewGuid();
        var validVideo = _fixture.DataGenerator.GetValidVideo();
        
        validVideo.AddCastMember(exampleId);
        validVideo.AddCastMember(exampleId2);

        validVideo.RemoveAllCastMembers();

        validVideo.CastMembers.Should().HaveCount(0);
    }
}