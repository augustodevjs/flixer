using MediatR;
using Flixer.Catalog.Application.UseCases.Genre.Common;

namespace Flixer.Catalog.Application.UseCases.Genre.UpdateGenre;

public interface IUpdateGenre : IRequestHandler<UpdateGenreInput, GenreModelOutput>
{ 
}