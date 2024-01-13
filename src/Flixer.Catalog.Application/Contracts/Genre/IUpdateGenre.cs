using MediatR;
using Flixer.Catalog.Application.Dtos.ViewModel.Genre;
using Flixer.Catalog.Application.Dtos.InputModel.Genre;

namespace Flixer.Catalog.Application.Contracts.Genre;

public interface IUpdateGenre : IRequestHandler<UpdateGenreInputModel, GenreViewModel>
{
}