using Flixer.Catalog.Domain.Extensions;

namespace Flixer.Catalog.Application.Common.Output.Video;

public class VideoOutput
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Title { get; private set; }
    public bool Published { get; private set; }
    public string Description { get; private set; }
    public string Rating { get; private set;}
    public int YearLaunched { get; private set; }
    public bool Opened { get; private set; }
    public int Duration { get; private set; }
    public IReadOnlyCollection<VideoOutputRelatedAggregate> Categories { get; private set; }
    public IReadOnlyCollection<VideoOutputRelatedAggregate> Genres { get; private set; }
    public IReadOnlyCollection<VideoOutputRelatedAggregate> CastMembers { get; private set; }

    public string? ThumbFileUrl { get; private set; }
    public string? BannerFileUrl { get; private set; }
    public string? ThumbHalfFileUrl { get; private set; }
    public string? VideoFileUrl { get; private set; }
    public string? TrailerFileUrl { get; private set; }

    public VideoOutput(
        Guid id,
        DateTime createdAt,
        string title,
        bool published,
        string description,
        string rating,
        int yearLaunched,
        bool opened,
        int duration,
        IReadOnlyCollection<VideoOutputRelatedAggregate> categories,
        IReadOnlyCollection<VideoOutputRelatedAggregate> genres,
        IReadOnlyCollection<VideoOutputRelatedAggregate> castMembers,
        string? thumbFileUrl,
        string? bannerFileUrl,
        string? thumbHalfFileUrl,
        string? videoFileUrl,
        string? trailerFileUrl)
    {
        Id = id;
        CreatedAt = createdAt;
        Title = title;
        Published = published;
        Description = description;
        Rating = rating;
        YearLaunched = yearLaunched;
        Opened = opened;
        Duration = duration;
        Categories = categories;
        Genres = genres;
        CastMembers = castMembers;
        ThumbFileUrl = thumbFileUrl;
        BannerFileUrl = bannerFileUrl;
        ThumbHalfFileUrl = thumbHalfFileUrl;
        VideoFileUrl = videoFileUrl;
        TrailerFileUrl = trailerFileUrl;
    }

    public static VideoOutput FromVideo(Domain.Entities.Video video)
    {
        return new VideoOutput(
            video.Id,
            video.CreatedAt,
            video.Title,
            video.Published,
            video.Description,
            video.Rating.ToStringSignal(),
            video.YearLaunched,
            video.Opened,
            video.Duration,
            video.Categories.Select(id => new VideoOutputRelatedAggregate(id)).ToList(),
            video.Genres.Select(id => new VideoOutputRelatedAggregate(id)).ToList(),
            video.CastMembers.Select(id => new VideoOutputRelatedAggregate(id)).ToList(),
            video.Thumb?.Path,
            video.Banner?.Path,
            video.ThumbHalf?.Path,
            video.Media?.FilePath,
            video.Trailer?.FilePath
        );
    }

    public static VideoOutput FromVideo(
        Domain.Entities.Video video,
        IReadOnlyList<Domain.Entities.Category>? categories = null,
        IReadOnlyCollection<Domain.Entities.Genre>? genres = null)
    {
        return new VideoOutput(
            video.Id,
            video.CreatedAt,
            video.Title,
            video.Published,
            video.Description,
            video.Rating.ToStringSignal(),
            video.YearLaunched,
            video.Opened,
            video.Duration,
            video.Categories.Select(id =>
                new VideoOutputRelatedAggregate(
                    id,
                    categories?.FirstOrDefault(category => category.Id == id)?.Name
                )).ToList(),
            video.Genres.Select(id =>
                new VideoOutputRelatedAggregate(
                    id,
                    genres?.FirstOrDefault(genre => genre.Id == id)?.Name
                )).ToList(),
            video.CastMembers.Select(id => new VideoOutputRelatedAggregate(id)).ToList(),
            video.Thumb?.Path,
            video.Banner?.Path,
            video.ThumbHalf?.Path,
            video.Media?.FilePath,
            video.Trailer?.FilePath
        );
    }
}

public class VideoOutputRelatedAggregate
{
    public Guid Id { get; }
    public string? Name { get; }

    public VideoOutputRelatedAggregate(Guid id, string? name = null)
    {
        Id = id;
        Name = name;
    }
}
