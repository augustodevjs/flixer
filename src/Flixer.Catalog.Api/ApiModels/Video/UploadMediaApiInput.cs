using Microsoft.AspNetCore.Mvc;
using Flixer.Catalog.Api.Extensions;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Common.Input.Video;

namespace Flixer.Catalog.Api.ApiModels.Video;

public class UploadMediaApiInput
{
    private static class MediaType
    {
        public const string Banner = "banner";
        public const string Thumb = "thumbnail";
        public const string ThumbHalf = "thumbnail_half";
        public const string Media = "video";
        public const string Trailer = "trailer";
    }

    [FromForm(Name = "media_file")]
    public IFormFile Media { get; set; }

    public UploadMediasInput ToUploadMediasInput(Guid id, string type)
        => type?.ToLower() switch
        {
            MediaType.Banner => new UploadMediasInput(id, bannerFile: Media.ToFileInput()),
            MediaType.Thumb => new UploadMediasInput(id, thumbFile: Media.ToFileInput()),
            MediaType.ThumbHalf => new UploadMediasInput(id, thumbHalfFile: Media.ToFileInput()),
            MediaType.Media => new UploadMediasInput(id, videoFile: Media.ToFileInput()),
            MediaType.Trailer => new UploadMediasInput(id, trailerFile: Media.ToFileInput()),
            _ => throw new EntityValidationException($"'{type}' is not a valid media type.", null)
        };
}