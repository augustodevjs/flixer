using MediatR;
using Flixer.Catalog.Application.Dtos.InputModel.Genre;

namespace Flixer.Catalog.Application.Contracts.Genre;

public interface IDeleteGenre : IRequestHandler<DeleteGenreInputModel>
{
}