using MediatR;

namespace Flixer.Catalog.Application.UseCases.Genre.DeleteGenre;

public interface IDeleteGenre : IRequestHandler<DeleteGenreInput>
{
}
