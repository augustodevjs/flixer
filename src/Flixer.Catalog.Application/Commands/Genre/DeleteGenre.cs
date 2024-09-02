using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Genre;

namespace Flixer.Catalog.Application.Commands.Genre;

public class DeleteGenre : IRequestHandler<DeleteGenreInput>
{
    private readonly IGenreRepository _genreRepository;
    private readonly ILogger<DeleteGenre> _logger;

    public DeleteGenre(
        IGenreRepository genreRepository, 
        ILogger<DeleteGenre> logger
    )
    {
        _logger = logger;
        _genreRepository = genreRepository;
    }

    public async Task Handle(DeleteGenreInput request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.GetById(request.Id);
        
        if (genre == null)
        {
            _logger.LogWarning("Genre with ID: {GenreId} not found.", request.Id);
            NotFoundException.ThrowIfNull(genre, $"Genre '{request.Id}' not found.");
        }

        _genreRepository.Delete(genre!);

        await _genreRepository.UnityOfWork.Commit();
        
        _logger.LogInformation("Genre with ID: {GenreId} deleted successfully.", request.Id);
    }
}