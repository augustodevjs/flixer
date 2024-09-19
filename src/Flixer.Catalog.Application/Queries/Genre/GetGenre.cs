using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Genre;
using Flixer.Catalog.Application.Common.Output.Genre;

namespace Flixer.Catalog.Application.Queries.Genre;

public class GetGenre : IRequestHandler<GetGenreInput, GenreOutput>
{
    private readonly ILogger<GetGenre> _logger;
    private readonly IGenreRepository _genreRepository;
    private readonly ICategoryRepository _categoryRepository;

    public GetGenre(
        ILogger<GetGenre> logger,
        IGenreRepository genreRepository, 
        ICategoryRepository categoryRepository
    )
    {
        _logger = logger;
        _genreRepository = genreRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<GenreOutput> Handle(GetGenreInput request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.GetById(request.Id);

        if (genre == null)
            NotFoundException.ThrowIfNull(genre, $"Genre '{request.Id}' not found.");

        var output = GenreOutput.FromGenre(genre!);

        if (output.Categories.Count <= 0) return output;
        
        var categories = (await _categoryRepository
                .GetListByIdsAsync(output.Categories.Select(x => x.Id)
                    .ToList()))
            .ToDictionary(x => x.Id);
            
        foreach (var category in output.Categories)
            category.Name = categories[category.Id].Name;
        
        _logger.LogInformation("Genre with ID: {GenreId} retrieved successfully.", request.Id);

        return output;
    }
}