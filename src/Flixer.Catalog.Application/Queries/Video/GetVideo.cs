using MediatR;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Video;
using Flixer.Catalog.Application.Common.Output.Video;

namespace Flixer.Catalog.Application.Queries.Video;

public class GetVideo : IRequestHandler<GetVideoInput, VideoOutput>
{
    private readonly IVideoRepository _videoRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly ICategoryRepository _categoryRepository;

    public GetVideo(
        IVideoRepository videoRepository, 
        IGenreRepository genreRepository, 
        ICategoryRepository categoryRepository
    )
    {
        _videoRepository = videoRepository;
        _genreRepository = genreRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<VideoOutput> Handle(GetVideoInput input, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetById(input.VideoId);
        
        if (video == null)
            NotFoundException.ThrowIfNull(video, $"Video '{video!.Id}' not found.");
        
        IReadOnlyList<Domain.Entities.Category>? categories = null;
        var relatedCategoriesIds = video.Categories?.Distinct().ToList();
        
        if (relatedCategoriesIds is not null && relatedCategoriesIds.Any())
            categories = await _categoryRepository.GetListByIdsAsync(relatedCategoriesIds);

        IReadOnlyList<Domain.Entities.Genre>? genres = null;
        var relatedGenresIds = video.Genres?.Distinct().ToList();
        
        if (relatedGenresIds is not null && relatedGenresIds.Count > 0)
            genres = await _genreRepository.GetListByIdsAsync(relatedGenresIds);
        
        return VideoOutput.FromVideo(video, categories, genres);
    }
}