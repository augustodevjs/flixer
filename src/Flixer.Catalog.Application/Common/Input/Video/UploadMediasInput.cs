using MediatR;
using Flixer.Catalog.Application.Common.Output.Common;

namespace Flixer.Catalog.Application.Common.Input.Video;

public class UploadMediasInput : IRequest
{
    public Guid VideoId { get; private set; }
    public FileInput? VideoFile { get; private set; }
    public FileInput? TrailerFile { get; private set; }

    public UploadMediasInput(Guid videoId, FileInput? videoFile, FileInput? trailerFile)
    {
        VideoId = videoId;
        VideoFile = videoFile;
        TrailerFile = trailerFile;
    }
}