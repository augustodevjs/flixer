using Xunit;
using FluentAssertions;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.UnitTest.Fixture.Domain;

namespace Flixer.Catalog.UnitTest.Domain.Entities;

[Collection(nameof(VideoFixture))]
public class MediaTest
{
    private readonly VideoFixture _fixture;

    public MediaTest(VideoFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    [Trait("Domain", "Media - Entities")]
    public void Media_ShouldHaveExpectedProperties_WhenInstantiated()
    {
        var expectedFilePath = _fixture.DataGenerator.GetValidMediaPath();
        
        var media = new Media(expectedFilePath);
        
        media.FilePath.Should().Be(expectedFilePath);
        media.Status.Should().Be(MediaStatus.Pending);
    }

    [Fact]
    [Trait("Domain", "Media - Entities")]
    public void Media_ShouldUpdateAsSentToEncode_WhenMethodIsCalled()
    {
        var media = _fixture.DataGenerator.GetValidMedia();

        media.UpdateAsSentToEncode();

        media.Status.Should().Be(MediaStatus.Processing);
    }
    
    [Fact]
    [Trait("Domain", "Media - Entities")]
    public void Media_ShouldUpdateAsEncoded_WhenMethodIsCalled()
    {
        var media = _fixture.DataGenerator.GetValidMedia();
        var encodedExamplePath = _fixture.DataGenerator.GetValidMediaPath();
        
        media.UpdateAsSentToEncode();
        media.UpdateAsEncoded(encodedExamplePath);

        media.Status.Should().Be(MediaStatus.Completed);
        media.EncodedPath.Should().Be(encodedExamplePath);
    }
    
    [Fact]
    [Trait("Domain", "Media - Entities")]
    public void Media_ShouldUpdateAsEncondingError_WhenMethodIsCalled()
    {
        var media = _fixture.DataGenerator.GetValidMedia();
        
        media.UpdateAsSentToEncode();
        media.UpdateAsEncodingError();

        media.Status.Should().Be(MediaStatus.Error);
        media.EncodedPath.Should().BeNull();

    }
}