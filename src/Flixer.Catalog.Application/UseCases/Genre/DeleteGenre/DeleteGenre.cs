using MediatR;
using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Application.Contracts;

namespace Flixer.Catalog.Application.UseCases.Genre.DeleteGenre;

public class DeleteGenre : IDeleteGenre
{
    private readonly IUnityOfWork _unitOfWork;
    private readonly IGenreRepository _genreRepository;

    public DeleteGenre(IUnityOfWork unitOfWork, IGenreRepository genreRepository)
    {
        _unitOfWork = unitOfWork;
        _genreRepository = genreRepository;
    }

    public async Task<Unit> Handle(DeleteGenreInput request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.Get(request.Id, cancellationToken);

        await _genreRepository.Delete(genre, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return Unit.Value;
    }
}
