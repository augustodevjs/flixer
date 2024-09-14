using MediatR;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Application.Common.Output.Video;
using Flixer.Catalog.Application.Common.Output.Common;

namespace Flixer.Catalog.Application.Common.Input.Video;

public class UpdateVideoInput : IRequest<VideoOutput>
{
    public Guid VideoId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int YearLaunched { get; private set; }
    public bool Opened { get; private set; }
    public bool Published { get; private set; }
    public int Duration { get; private set; }
    public Rating Rating { get; private set; }
    public List<Guid>? GenresIds { get; private set; }
    public List<Guid>? CategoriesIds { get; private set; }
    public List<Guid>? CastMembersIds { get; private set; }
    public FileInput? Banner { get; private set; }
    public FileInput? Thumb { get; private set; }
    public FileInput? ThumbHalf { get; private set; }

    public UpdateVideoInput(
        Guid videoId,
        string title,
        string description,
        int yearLaunched,
        bool opened,
        bool published,
        int duration,
        Rating rating,
        List<Guid>? genresIds = null,
        List<Guid>? categoriesIds = null,
        List<Guid>? castMembersIds = null,
        FileInput? banner = null,
        FileInput? thumb = null,
        FileInput? thumbHalf = null
    )
    {
        VideoId = videoId;
        Title = title;
        Description = description;
        YearLaunched = yearLaunched;
        Opened = opened;
        Published = published;
        Duration = duration;
        Rating = rating;
        GenresIds = genresIds;
        CategoriesIds = categoriesIds;
        CastMembersIds = castMembersIds;
        Banner = banner;
        Thumb = thumb;
        ThumbHalf = thumbHalf;
    }
}
