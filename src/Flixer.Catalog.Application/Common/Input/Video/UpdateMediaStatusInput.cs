using MediatR;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Application.Common.Output.Video;

namespace Flixer.Catalog.Application.Common.Input.Video;

public class UpdateMediaStatusInput : IRequest<VideoOutput>
{
    public Guid VideoId { get; private set; }
    public MediaStatus Status { get; private set; }
    public string? EncodedPath { get; private set; }
    public string? ErrorMessage { get; private set; }

    public UpdateMediaStatusInput(
        Guid videoId,
        MediaStatus status,
        string? encodedPath = null,
        string? errorMessage = null)
    {
        VideoId = videoId;
        Status = status;
        EncodedPath = encodedPath;
        ErrorMessage = errorMessage;
    }
}