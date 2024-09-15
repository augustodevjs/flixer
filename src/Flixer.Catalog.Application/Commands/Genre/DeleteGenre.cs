using MediatR;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Genre;

namespace Flixer.Catalog.Application.Commands.Genre;

public class DeleteGenre : IRequestHandler<DeleteGenreInput>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteGenre> _logger;
    private readonly IGenreRepository _genreRepository;

    public DeleteGenre(
        IUnitOfWork unitOfWork,
        ILogger<DeleteGenre> logger, 
        IGenreRepository genreRepository
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _genreRepository = genreRepository;
    }

    public async Task Handle(DeleteGenreInput request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.GetById(request.Id);

        if (genre == null)
            NotFoundException.ThrowIfNull(genre, $"Genre '{request.Id}' not found.");

        _genreRepository.Delete(genre!);

        await _unitOfWork.Commit();
        
        _logger.LogInformation("Genre with ID: {GenreId} deleted successfully.", request.Id);
    }
}