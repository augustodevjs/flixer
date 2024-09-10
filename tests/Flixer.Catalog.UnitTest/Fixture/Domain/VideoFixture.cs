using Xunit;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Domain;

[CollectionDefinition(nameof(VideoFixture))]
public class VideoFixtureCollection : ICollectionFixture<VideoFixture>
{
    
}

public class VideoFixture
{
    public VideoDataGenerator DataGenerator { get; } = new();
}