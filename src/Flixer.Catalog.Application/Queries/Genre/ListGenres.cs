using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Genre;
using Flixer.Catalog.Application.Common.Output.Genre;

namespace Flixer.Catalog.Application.Queries.Genre;

public class ListGenres : IRequestHandler<ListGenresInput, ListGenresOutput>
{
    private readonly ILogger<ListGenres> _logger;
    private readonly IGenreRepository _genreRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ListGenres(
        ILogger<ListGenres> logger, 
        IGenreRepository genreRepository,
        ICategoryRepository categoryRepository
    )
    {
        _logger = logger;
        _genreRepository = genreRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ListGenresOutput> Handle(ListGenresInput request, CancellationToken cancellationToken)
    {
        var searchInput = request.ToSearchInput();

        var listGenres = await _genreRepository.Search(searchInput);

        var genresListOutput = ListGenresOutput.FromSearchOutput(listGenres);

        var relatedCategoriesIds = listGenres.Items
            .SelectMany(genre => genre.Categories)
            .Distinct()
            .ToList();
        
        if (relatedCategoriesIds.Count <= 0)
        {
            _logger.LogInformation("No related categories found. Returning genres list.");
            return genresListOutput;
        }

        var listCategories = await _categoryRepository.GetListByIdsAsync(relatedCategoriesIds);

        genresListOutput.FillWithCategoryNames(listCategories);
        
        _logger.LogInformation("Search completed successfully with {TotalItems} total items.", listGenres.Total);

        return genresListOutput;
    }
}
