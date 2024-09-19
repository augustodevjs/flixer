using MediatR;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Application.Intefaces;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Video;

namespace Flixer.Catalog.Application.Commands.Video;

public class DeleteVideo : IRequestHandler<DeleteVideoInput>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;
    private readonly IVideoRepository _videoRepository;

    public DeleteVideo(
        IUnitOfWork unitOfWork,
        IStorageService storageService, 
        IVideoRepository videoRepository
    )
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _videoRepository = videoRepository;
    }

    public async Task Handle(DeleteVideoInput input, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetById(input.Id);

        if (video == null)
            NotFoundException.ThrowIfNull(video, $"Video '{video!.Id}' not found.");
        
        var trailerFilePath = video.Trailer?.FilePath;
        var mediaFilePath = video.Media?.FilePath;
        
        _videoRepository.Delete(video);
        await _unitOfWork.Commit();

        await ClearVideoMedias(mediaFilePath, trailerFilePath);
        await ClearImageMedias(video!.Banner?.Path, video!.Thumb?.Path, video!.ThumbHalf?.Path);
    }
    
    private async Task ClearImageMedias(
        string? bannerFilePath,
        string? thumbFilePath,
        string? thumbHalfFilePath
    )
    {
        if (bannerFilePath is not null)
            await _storageService.Delete(bannerFilePath);

        if (thumbFilePath is not null)
            await _storageService.Delete(thumbFilePath);

        if (thumbHalfFilePath is not null)
            await _storageService.Delete(thumbHalfFilePath);
    }

    private async Task ClearVideoMedias(
        string? mediaFilePath,
        string? trailerFilePath
    )
    {
        if (trailerFilePath is not null)
            await _storageService.Delete(trailerFilePath);

        if (mediaFilePath is not null)
            await _storageService.Delete(mediaFilePath);
    }
}