using MediatR;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Application.Intefaces;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Video;
using Flixer.Catalog.Application.Common.Input.Common;

namespace Flixer.Catalog.Application.Commands.Video;

public class UploadMedia : IRequestHandler<UploadMediasInput>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;
    private readonly IVideoRepository _videoRepository;

    public UploadMedia(
        IUnitOfWork unitOfWork,
        IStorageService storageService, 
        IVideoRepository videoRepository
    )
    {
        _unitOfWork = unitOfWork;
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
            await UploadImages(input, video);

            _videoRepository.Update(video);
            await _unitOfWork.Commit();
        }
        catch (Exception)
        {
            await ClearStorage(input, video);
            throw;
        }
    }   

    private async Task UploadImages(UploadMediasInput input, Domain.Entities.Video video)
    {
        if (input.BannerFile is not null)
        {
            var fileName = StorageFileName
                .Create(video.Id, nameof(video.Banner), input.BannerFile.Extension);
            
            var uploadedFilePath = await _storageService.Upload(
                fileName,
                input.BannerFile.ContentType,
                input.BannerFile.FileStream
            );
            
            video.UpdateBanner(uploadedFilePath);
        }

        if (input.ThumbFile is not null)
        {
            var fileName = StorageFileName
                .Create(video.Id, nameof(video.Thumb), input.ThumbFile.Extension);
            
            var uploadedFilePath = await _storageService.Upload(
                fileName,
                input.ThumbFile.ContentType,
                input.ThumbFile.FileStream
            );
            
            video.UpdateThumb(uploadedFilePath);
        }

        if (input.ThumbHalfFile is not null)
        {
            var fileName = StorageFileName
                .Create(video.Id, nameof(video.ThumbHalf), input.ThumbHalfFile.Extension);
            
            var uploadedFilePath = await _storageService.Upload(
                fileName,
                input.ThumbHalfFile.ContentType,
                input.ThumbHalfFile.FileStream
            );
            
            video.UpdateThumbHalf(uploadedFilePath);
        }
    }

    private async Task ClearStorage(UploadMediasInput input, Domain.Entities.Video video)
    {
        if (input.VideoFile is not null && video.Media is not null)
            await _storageService.Delete(video.Media.FilePath);
        
        if (input.TrailerFile is not null && video.Trailer is not null)
            await _storageService.Delete(video.Trailer.FilePath);
        
        if (input.BannerFile is not null && video.Banner is not null)
            await _storageService.Delete(video.Banner.Path);
        
        if (input.ThumbFile is not null && video.Thumb is not null)
            await _storageService.Delete(video.Thumb.Path);
        
        if (input.ThumbHalfFile is not null && video.ThumbHalf is not null)
            await _storageService.Delete(video.ThumbHalf.Path);
    }

    private async Task UploadTrailer(UploadMediasInput input, Domain.Entities.Video video)
    {
        if (input.TrailerFile is not null)
        {
            var fileName = StorageFileName.Create(video.Id, nameof(video.Trailer), input.TrailerFile.Extension);
            
            var uploadedFilePath = await _storageService.
                Upload(fileName, input.TrailerFile!.ContentType, input.TrailerFile.FileStream);
            
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
                Upload(fileName, input.VideoFile!.ContentType, input.VideoFile.FileStream);
            
            video.UpdateMedia(uploadedFilePath);
        }
    }
}