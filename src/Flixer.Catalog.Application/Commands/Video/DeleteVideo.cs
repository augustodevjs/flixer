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
        
        _videoRepository.Delete(video);
        await _unitOfWork.Commit();

        if (video.Trailer is not null)
            await _storageService.Delete(video.Trailer.FilePath);

        if (video.Media is not null)
            await _storageService.Delete(video.Media.FilePath);
    }
}