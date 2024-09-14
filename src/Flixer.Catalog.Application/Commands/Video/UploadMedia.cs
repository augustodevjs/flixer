using MediatR;
using Flixer.Catalog.Application.Intefaces;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Video;
using Flixer.Catalog.Application.Common.Input.Common;

namespace Flixer.Catalog.Application.Commands.Video;

public class UploadMedia : IRequestHandler<UploadMediasInput>
{
    private readonly IStorageService _storageService;
    private readonly IVideoRepository _videoRepository;

    public UploadMedia(IStorageService storageService, IVideoRepository videoRepository)
    {
        _storageService = storageService;
        _videoRepository = videoRepository;
    }

    public async Task Handle(UploadMediasInput input, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetById(input.VideoId);
        
        if (video == null)
            NotFoundException.ThrowIfNull(video, $"Video '{video!.Id}' not found.");
        
        try
        {
            await UploadVideo(input, video);
            await UploadTrailer(input, video);

            _videoRepository.Update(video);
            await _videoRepository.UnityOfWork.Commit();
        }
        catch (Exception)
        {
            await ClearStorage(input, video);
            throw;
        }
    }
    
    private async Task ClearStorage(UploadMediasInput input, Domain.Entities.Video video)
    {
        if (input.VideoFile is not null && video.Media is not null)
            await _storageService.Delete(video.Media.FilePath);
        
        if (input.TrailerFile is not null && video.Trailer is not null)
            await _storageService.Delete(video.Trailer.FilePath);
    }

    private async Task UploadTrailer(UploadMediasInput input, Domain.Entities.Video video)
    {
        if (input.TrailerFile is not null)
        {
            var fileName = StorageFileName.Create(video.Id, nameof(video.Trailer), input.TrailerFile.Extension);
            
            var uploadedFilePath = await _storageService.
                Upload(fileName, input.TrailerFile.FileStream);
            
            video.UpdateTrailer(uploadedFilePath);
        }
    }

    private async Task UploadVideo(UploadMediasInput input, Domain.Entities.Video video)
    {
        if (input.VideoFile is not null)
        {
            var fileName = StorageFileName
                .Create(video.Id, nameof(video.Media), input.VideoFile.Extension);
            
            var uploadedFilePath = await _storageService.
                Upload(fileName, input.VideoFile.FileStream);
            
            video.UpdateMedia(uploadedFilePath);
        }
    }
}