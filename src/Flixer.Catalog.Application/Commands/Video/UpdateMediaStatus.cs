using MediatR;
using Flixer.Catalog.Domain.Enums;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Video;
using Flixer.Catalog.Application.Common.Output.Video;

namespace Flixer.Catalog.Application.Commands.Video;

public class UpdateMediaStatus: IRequestHandler<UpdateMediaStatusInput, VideoOutput>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVideoRepository _videoRepository;
    private readonly ILogger<UpdateMediaStatus> _logger;

    public UpdateMediaStatus(
        IUnitOfWork unitOfWork, 
        IVideoRepository videoRepository, 
        ILogger<UpdateMediaStatus> logger
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _videoRepository = videoRepository;
    }

    public async Task<VideoOutput> Handle(UpdateMediaStatusInput request, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetById(request.VideoId);
        
        if (video == null)
            NotFoundException.ThrowIfNull(video, $"Video '{video!.Id}' not found.");

        switch (request.Status)
        {
            case MediaStatus.Completed:
                video.UpdateAsEncoded(request.EncodedPath!);
                break;
            case MediaStatus.Error:
                _logger.LogError(
                    "There was an error while trying to encode the video {videoId}: {error}",
                    video.Id, request.ErrorMessage);
                video.UpdateAsEncodingError();
                break;
            default:
                throw new EntityValidationException("Invalid media status", null);
        }

        _videoRepository.Update(video);
        await _unitOfWork.Commit();
        
        return VideoOutput.FromVideo(video);
    }
}