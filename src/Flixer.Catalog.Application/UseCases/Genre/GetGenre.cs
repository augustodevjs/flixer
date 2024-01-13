using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Application.Dtos.ViewModel.Genre;
using Flixer.Catalog.Application.Dtos.InputModel.Genre;
using Flixer.Catalog.Application.Contracts.UseCases.Genre;

namespace Flixer.Catalog.Application.UseCases.Genre;

public class GetGenre : IGetGenre
{
    private readonly IGenreRepository _genreRepository;

    public GetGenre(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<GenreViewModel> Handle(GetGenreInputModel request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.Get(request.Id, cancellationToken);

        if (genre == null)
        {
            NotFoundException.ThrowIfNull(genre, $"Genre '{request.Id}' not found.");
        }

        return GenreViewModel.FromGenre(genre!);
    }
}