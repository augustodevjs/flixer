using MediatR;
using Flixer.Catalog.Application.UseCases.Genre.Common;

namespace Flixer.Catalog.Application.UseCases.Genre.CreateGenre;

public interface ICreateGenre : IRequestHandler<CreateGenreInput, GenreModelOutput>
{
}
