using MediatR;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Application.Common.Output.Common;
using Flixer.Catalog.Application.Common.Output.Video;

namespace Flixer.Catalog.Application.Common.Input.Video;

public class CreateVideoInput : IRequest<VideoOutput>
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int YearLaunched { get; private set; }
    public bool Opened { get; private set; }
    public bool Published { get; private set; }
    public int Duration { get; private set; }
    public Rating Rating { get; private set; }
    public IReadOnlyCollection<Guid>? CategoriesIds { get; private set; }
    public IReadOnlyCollection<Guid>? GenresIds { get; private set; }
    public IReadOnlyCollection<Guid>? CastMembersIds { get; private set; }
    public FileInput? Thumb { get; private set; }
    public FileInput? Banner { get; private set; }
    public FileInput? ThumbHalf { get; private set; }
    public FileInput? Media { get; private set; }
    public FileInput? Trailer { get; private set; }

    public CreateVideoInput(
        string title,
        string description,
        int yearLaunched,
        bool opened,
        bool published,
        int duration,
        Rating rating,
        IReadOnlyCollection<Guid>? categoriesIds = null,
        IReadOnlyCollection<Guid>? genresIds = null,
        IReadOnlyCollection<Guid>? castMembersIds = null,
        FileInput? thumb = null,
        FileInput? banner = null,
        FileInput? thumbHalf = null,
        FileInput? media = null,
        FileInput? trailer = null)
    {
        Title = title;
        Description = description;
        YearLaunched = yearLaunched;
        Opened = opened;
        Published = published;
        Duration = duration;
        Rating = rating;
        CategoriesIds = categoriesIds;
        GenresIds = genresIds;
        CastMembersIds = castMembersIds;
        Thumb = thumb;
        Banner = banner;
        ThumbHalf = thumbHalf;
        Media = media;
        Trailer = trailer;
    }
}
