using MediatR;
using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.Dtos.InputModel.Genre;
using Flixer.Catalog.Application.Contracts.UseCases.Genre;

namespace Flixer.Catalog.Application.UseCases.Genre;

public class DeleteGenre : IDeleteGenre
{
    private readonly IUnityOfWork _unitOfWork;
    private readonly IGenreRepository _genreRepository;

    public DeleteGenre(IUnityOfWork unitOfWork, IGenreRepository genreRepository)
    {
        _unitOfWork = unitOfWork;
        _genreRepository = genreRepository;
    }

    public async Task<Unit> Handle(DeleteGenreInputModel request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.Get(request.Id, cancellationToken);

        if (genre == null)
        {
            NotFoundException.ThrowIfNull(genre, $"Genre '{request.Id}' not found.");
        }

        await _genreRepository.Delete(genre!, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return Unit.Value;
    }
}