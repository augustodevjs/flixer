using MediatR;
using Flixer.Catalog.Application.Common.Output.Video;

namespace Flixer.Catalog.Application.Common.Input.Video;

public class GetVideoInput : IRequest<VideoOutput>
{
    public Guid VideoId { get; private set; }
    
    public GetVideoInput(Guid videoId)
    {
        VideoId = videoId;
    }
}