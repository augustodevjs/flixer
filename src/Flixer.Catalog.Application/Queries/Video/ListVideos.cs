using MediatR;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Video;
using Flixer.Catalog.Application.Common.Output.Video;

namespace Flixer.Catalog.Application.Queries.Video;

public class ListVideos : IRequestHandler<ListVideosInput, ListVideosOutput>
{
    private readonly IGenreRepository _genreRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ListVideos(
        IGenreRepository genreRepository, 
        IVideoRepository videoRepository, 
        ICategoryRepository categoryRepository
    )
    {
        _genreRepository = genreRepository;
        _videoRepository = videoRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ListVideosOutput> Handle(ListVideosInput input, CancellationToken cancellationToken)
    {
        var result = await _videoRepository.Search(input.ToSearchInput());
        
        IReadOnlyList<Domain.Entities.Category>? categories = null;
        
        var relatedCategoriesIds = result.Items
            .SelectMany(video => video.Categories).Distinct().ToList();
        
        if (relatedCategoriesIds.Count > 0)
            categories = await _categoryRepository.GetListByIdsAsync(relatedCategoriesIds);

        IReadOnlyList<Domain.Entities.Genre>? genres = null;
        
        var relatedGenresIds = result.Items
            .SelectMany(item => item.Genres).Distinct().ToList();
        
        if (relatedGenresIds.Count > 0)
            genres = await _genreRepository.GetListByIdsAsync(relatedGenresIds);

        var output = new ListVideosOutput(
            result.CurrentPage, 
            result.PerPage,
            result.Total,
            result.Items.Select(item => VideoOutput.FromVideo(item, categories, genres)).ToList());
        
        return output;
    }
}