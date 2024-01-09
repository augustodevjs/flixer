using MediatR;
using Flixer.Catalog.Application.UseCases.Genre.Common;

namespace Flixer.Catalog.Application.UseCases.Genre.GetGenre;

public class GetGenreInput : IRequest<GenreModelOutput>
{
    public Guid Id { get; set; }

    public GetGenreInput(Guid id)
        => Id = id;
}