using MediatR;

namespace Flixer.Catalog.Application.UseCases.Genre.ListGenres;

public interface IListGenres : IRequestHandler<ListGenresInput, ListGenresOutput>
{ 
}