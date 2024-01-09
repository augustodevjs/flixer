using MediatR;
using Flixer.Catalog.Application.UseCases.Genre.Common;

namespace Flixer.Catalog.Application.UseCases.Genre.GetGenre;

public interface IGetGenre : IRequestHandler<GetGenreInput, GenreModelOutput>
{
}
