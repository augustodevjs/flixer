using MediatR;
using Flixer.Catalog.Application.Common.Output.Common;

namespace Flixer.Catalog.Application.Common.Input.Video;

public class UploadMediasInput : IRequest
{
    public Guid VideoId { get; private set; }
    public FileInput? VideoFile { get; private set; }
    public FileInput? TrailerFile { get; private set; }
    public FileInput? BannerFile { get; private set; }
    public FileInput? ThumbFile { get; private set; }
    public FileInput? ThumbHalfFile { get; private set; }

    public UploadMediasInput(
        Guid videoId,
        FileInput? videoFile = null,
        FileInput? thumbFile = null,
        FileInput? bannerFile = null,
        FileInput? trailerFile = null,
        FileInput? thumbHalfFile = null
    )
    {
        VideoId = videoId;
        VideoFile = videoFile;
        ThumbFile = thumbFile;
        BannerFile = bannerFile;
        TrailerFile = trailerFile;
        ThumbHalfFile = thumbHalfFile;
    }
}