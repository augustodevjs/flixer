using MediatR;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Application.Intefaces;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Video;
using Flixer.Catalog.Application.Common.Input.Common;
using Flixer.Catalog.Application.Common.Output.Video;

namespace Flixer.Catalog.Application.Commands.Video;

public class CreateVideo : IRequestHandler<CreateVideoInput, VideoOutput>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;
    private readonly IGenreRepository _genreRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICastMemberRepository _castMemberRepository;

    public CreateVideo(
        IUnitOfWork unitOfWork,
        IStorageService storageService, 
        IGenreRepository genreRepository, 
        IVideoRepository videoRepository, 
        ICategoryRepository categoryRepository, 
        ICastMemberRepository castMemberRepository
    )
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _genreRepository = genreRepository;
        _videoRepository = videoRepository;
        _categoryRepository = categoryRepository;
        _castMemberRepository = castMemberRepository;
    }

    public async Task<VideoOutput> Handle(CreateVideoInput input, CancellationToken cancellationToken)
    {
        var video = new Domain.Entities.Video(
            input.Title,
            input.Description,
            input.YearLaunched,
            input.Opened,
            input.Published,
            input.Duration,
            input.Rating
        );

        await ValidateAndAddRelations(input, video);

        try
        {
            await UploadImagesMedia(input, video);
            await UploadVideosMedia(input, video);

            _videoRepository.Create(video);
            await _unitOfWork.Commit();

            return VideoOutput.FromVideo(video);
        }
        catch (Exception)
        {
            await ClearStorage(video);
            throw;
        }
    }
    
    private async Task ClearStorage(Domain.Entities.Video video)
    {
        if (video.Thumb is not null)
            await _storageService.Delete(video.Thumb.Path);
        
        if (video.ThumbHalf is not null)
            await _storageService.Delete(video.ThumbHalf.Path);
        
        if (video.Banner is not null)
            await _storageService.Delete(video.Banner.Path);
        
        if (video.Media is not null)
            await _storageService.Delete(video.Media.FilePath);
        
        if (video.Trailer is not null)
            await _storageService.Delete(video.Trailer.FilePath);
    }

    private async Task UploadImagesMedia(CreateVideoInput input, Domain.Entities.Video video)
    {
        if (input.Thumb is not null)
        {
            var fileName = StorageFileName
                .Create(video.Id, nameof(video.Thumb), input.Thumb.Extension);
            
            var thumbUrl = await _storageService
                .Upload(fileName, input.Thumb.ContentType, input.Thumb.FileStream);
            
            video.UpdateThumb(thumbUrl);
        }

        if (input.Banner is not null)
        {
            var fileName = StorageFileName
                .Create(video.Id, nameof(video.Banner), input.Banner.Extension);
            
            var bannerUrl = await _storageService
                .Upload(fileName, input.Banner!.ContentType, input.Banner.FileStream);
            
            video.UpdateBanner(bannerUrl);
        }

        if (input.ThumbHalf is not null)
        {
            var fileName = StorageFileName
                .Create(video.Id, nameof(video.ThumbHalf), input.ThumbHalf.Extension);
            
            var thumbHalfUrl = await _storageService
                .Upload(fileName, input.ThumbHalf!.ContentType, input.ThumbHalf.FileStream);
            
            video.UpdateThumbHalf(thumbHalfUrl);
        }
    }

    private async Task UploadVideosMedia(CreateVideoInput input, Domain.Entities.Video video)
    {
        if (input.Media is not null)
        {
            var fileName = StorageFileName
                .Create(video.Id, nameof(video.Media), input.Media.Extension);
            
            var mediaUrl = await _storageService
                .Upload(fileName, input.Media!.ContentType, input.Media.FileStream);
            
            video.UpdateMedia(mediaUrl);
        }
        
        if (input.Trailer is not null)
        {
            var fileName = StorageFileName
                .Create(video.Id, nameof(video.Trailer), input.Trailer.Extension);
            
            var mediaUrl = await _storageService
                .Upload(fileName,  input.Trailer!.ContentType, input.Trailer.FileStream);
            
            video.UpdateTrailer(mediaUrl);
        }
    }

    private async Task ValidateAndAddRelations(CreateVideoInput input, Domain.Entities.Video video)
    {
        if ((input.CategoriesIds?.Count ?? 0) > 0)
        {
            await ValidateCategoriesIds(input);
            input.CategoriesIds!.ToList().ForEach(video.AddCategory);
        }

        if ((input.GenresIds?.Count ?? 0) > 0)
        {
            await ValidateGenresIds(input);
            input.GenresIds!.ToList().ForEach(video.AddGenre);
        }

        if ((input.CastMembersIds?.Count ?? 0) > 0)
        {
            await ValidateCastMembersIds(input);
            input.CastMembersIds!.ToList().ForEach(video.AddCastMember);
        }
    }

    private async Task ValidateCastMembersIds(CreateVideoInput input)
    {
        var persistenceIds = await _castMemberRepository
            .GetIdsListByIds(input.CastMembersIds!.ToList());
        
        if (persistenceIds.Count < input.CastMembersIds!.Count)
        {
            var notFoundIds = input.CastMembersIds!.ToList()
                .FindAll(id => !persistenceIds.Contains(id));
            
            throw new RelatedAggregateException(
                $"Related cast member id (or ids) not found: {string.Join(',', notFoundIds)}.");
        }
    }

    private async Task ValidateGenresIds(CreateVideoInput input)
    {
        var persistenceIds = await _genreRepository
            .GetIdsListByIdsAsync(input.GenresIds!.ToList());
        
        if (persistenceIds.Count < input.GenresIds!.Count)
        {
            var notFoundIds = input.GenresIds!.ToList()
                .FindAll(id => !persistenceIds.Contains(id));
            
            throw new RelatedAggregateException(
                $"Related genre id (or ids) not found: {string.Join(',', notFoundIds)}.");
        }
    }

    private async Task ValidateCategoriesIds(CreateVideoInput input)
    {
        var persistenceIds = await _categoryRepository
            .GetIdsListByIds(input.CategoriesIds!.ToList());
        
        if (persistenceIds.Count < input.CategoriesIds!.Count)
        {
            var notFoundIds = input.CategoriesIds!.ToList()
                .FindAll(categoryId => !persistenceIds.Contains(categoryId));
            
            throw new RelatedAggregateException(
                $"Related category id (or ids) not found: {string.Join(',', notFoundIds)}.");
        }
    }
}